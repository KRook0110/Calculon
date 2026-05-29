#!/usr/bin/env python3
import os
import re
import sys

# C# access and other modifiers to strip out from the type names
MODIFIERS_SET = {
    'public', 'private', 'protected', 'internal', 'static', 'readonly', 
    'volatile', 'virtual', 'override', 'new', 'abstract', 'sealed', 
    'async', 'unsafe', 'extern', 'partial', 'event', 'delegate', 'const', 'enum'
}

class CSharpContainer:
    def __init__(self, name, kind, depth, bases=None):
        self.name = name
        self.kind = kind  # 'class', 'struct', or 'interface'
        self.depth = depth
        self.bases = bases if bases else []
        self.variables = []
        self.functions = []

def preprocess_csharp(code):
    """
    Remove comments, preprocessor directives, string literals, and char literals
    from C# code to avoid matching braces and keywords inside them.
    """
    output = []
    i = 0
    n = len(code)
    state = "NORMAL"
    
    while i < n:
        char = code[i]
        next_char = code[i+1] if i + 1 < n else ""
        
        if state == "NORMAL":
            if char == '/' and next_char == '/':
                state = "IN_LINE_COMMENT"
                i += 2
            elif char == '/' and next_char == '*':
                state = "IN_BLOCK_COMMENT"
                i += 2
            elif char == '@' and next_char == '"':
                state = "IN_VERBATIM_STRING"
                output.append('"')
                output.append('"')
                i += 2
            elif char == '"':
                state = "IN_STRING"
                output.append('"')
                output.append('"')
                i += 1
            elif char == "'":
                state = "IN_CHAR"
                output.append("'")
                output.append("'")
                i += 1
            elif char == '#' and (i == 0 or code[i-1] == '\n' or code[i-1].isspace()):
                state = "IN_PREPROCESSOR"
                i += 1
            else:
                output.append(char)
                i += 1
        elif state == "IN_LINE_COMMENT":
            if char == '\n':
                state = "NORMAL"
                output.append('\n')
            i += 1
        elif state == "IN_BLOCK_COMMENT":
            if char == '*' and next_char == '/':
                state = "NORMAL"
                i += 2
            else:
                i += 1
        elif state == "IN_STRING":
            if char == '\\':
                i += 2
            elif char == '"':
                state = "NORMAL"
                i += 1
            else:
                i += 1
        elif state == "IN_VERBATIM_STRING":
            if char == '"' and next_char == '"':
                i += 2
            elif char == '"':
                state = "NORMAL"
                i += 1
            else:
                i += 1
        elif state == "IN_CHAR":
            if char == '\\':
                i += 2
            elif char == "'":
                state = "NORMAL"
                i += 1
            else:
                i += 1
        elif state == "IN_PREPROCESSOR":
            if char == '\n':
                state = "NORMAL"
                output.append('\n')
            i += 1
            
    return "".join(output)

def split_by_top_level_commas(s):
    """Split a string by commas that are not nested within angle brackets or parentheses."""
    parts = []
    current = []
    angle_depth = 0
    paren_depth = 0
    for char in s:
        if char == '<':
            angle_depth += 1
            current.append(char)
        elif char == '>':
            angle_depth -= 1
            current.append(char)
        elif char == '(':
            paren_depth += 1
            current.append(char)
        elif char == ')':
            paren_depth -= 1
            current.append(char)
        elif char == ',' and angle_depth == 0 and paren_depth == 0:
            parts.append("".join(current).strip())
            current = []
        else:
            current.append(char)
    if current:
        parts.append("".join(current).strip())
    return parts

def parse_container_declaration(segment_str):
    """Detect and parse class, struct, or interface declarations along with inheritance."""
    clean_seg = re.sub(r'\[[^\]]*\]', '', segment_str).strip()
    match = re.search(r'\b(class|struct|interface)\s+([A-Za-z0-9_<>]+)', clean_seg)
    if not match:
        return None
        
    kind = match.group(1)
    name = match.group(2)
    
    # Parse inheritance/interfaces
    bases = []
    if ':' in clean_seg:
        parts = clean_seg.split(':', 1)[1].strip()
        raw_bases = split_by_top_level_commas(parts)
        for base in raw_bases:
            # Clean up base name (get first word in case of constraints)
            base = base.strip().split()[0]
            if base:
                bases.append(base)
                
    return {
        'kind': kind,
        'name': name,
        'bases': bases
    }

def parse_variable(clean_part):
    """Parse field or property declaration, returning its access, name, and type."""
    if '=>' in clean_part:
        clean_part = clean_part.split('=>')[0].strip()
    if '=' in clean_part:
        clean_part = clean_part.split('=')[0].strip()
        
    tokens = clean_part.split()
    if not tokens or len(tokens) < 2:
        return None
        
    name = tokens[-1]
    type_tokens = tokens[:-1]
    
    access = '-'
    actual_type_tokens = []
    
    for token in type_tokens:
        if token == 'public':
            access = '+'
        elif token in MODIFIERS_SET:
            pass
        else:
            actual_type_tokens.append(token)
            
    if not actual_type_tokens:
        return None
        
    data_type = " ".join(actual_type_tokens)
    return {
        'access': access,
        'name': name,
        'type': data_type
    }

def parse_variable_segment(clean_segment):
    """Parse a clean variable segment which may contain multiple variables separated by commas."""
    parts = split_by_top_level_commas(clean_segment)
    variables = []
    first_var = None
    
    for part in parts:
        part = part.strip()
        if not part:
            continue
            
        clean_part = part
        if '=>' in clean_part:
            clean_part = clean_part.split('=>')[0].strip()
        if '=' in clean_part:
            clean_part = clean_part.split('=')[0].strip()
            
        tokens = clean_part.split()
        if not tokens:
            continue
            
        if first_var is None:
            var_info = parse_variable(part)
            if var_info:
                first_var = var_info
                variables.append(var_info)
        else:
            name = tokens[-1]
            variables.append({
                'access': first_var['access'],
                'name': name,
                'type': first_var['type']
            })
    return variables

def parse_method(clean_seg):
    """Parse method signature, returning its access and name."""
    if '=>' in clean_seg:
        clean_seg = clean_seg.split('=>')[0].strip()
        
    if '(' not in clean_seg:
        return None
        
    header = clean_seg.split('(')[0].strip()
    tokens = header.split()
    if not tokens:
        return None
        
    name = tokens[-1]
    type_tokens = tokens[:-1]
    
    access = '-'
    for token in type_tokens:
        if token == 'public':
            access = '+'
            break
            
    return {
        'access': access,
        'name': name
    }

def parse_csharp_file(file_content):
    """Parse a single C# file and return a list of CSharpContainer objects."""
    clean_code = preprocess_csharp(file_content)
    
    # Segment code by delimiters ';', '{', '}' while tracking brace depth
    segments = []
    buffer = []
    brace_depth = 0
    
    for char in clean_code:
        if char in ('{', '}', ';'):
            segment_str = "".join(buffer).strip()
            segments.append((segment_str, char, brace_depth))
            buffer = []
            if char == '{':
                brace_depth += 1
            elif char == '}':
                brace_depth -= 1
        else:
            buffer.append(char)
            
    containers = []
    container_stack = []
    
    for segment_str, delimiter, depth in segments:
        if not segment_str and delimiter != '}':
            continue
            
        # Check if it is a class/struct/interface declaration
        if delimiter == '{':
            container_info = parse_container_declaration(segment_str)
            if container_info:
                new_container = CSharpContainer(container_info['name'], container_info['kind'], depth, container_info['bases'])
                containers.append(new_container)
                container_stack.append(new_container)
                continue
                
        # If we see a closing brace, check if we are closing a container
        if delimiter == '}':
            if container_stack and container_stack[-1].depth == depth - 1:
                container_stack.pop()
                continue
                
        # If we are inside an active container, parse its members at exactly (container_depth + 1)
        if container_stack:
            active_container = container_stack[-1]
            if depth == active_container.depth + 1:
                # Strip attributes first to avoid matching '(' inside them (e.g. [Header("Name")])
                clean_segment = re.sub(r'\[[^\]]*\]', '', segment_str).strip()
                if not clean_segment:
                    continue
                
                # Check if it contains '(' before any '=' (which means it's a method)
                part_before_eq = clean_segment.split('=')[0].strip()
                if '(' in part_before_eq:
                    method_info = parse_method(clean_segment)
                    if method_info:
                        active_container.functions.append(method_info)
                else:
                    var_infos = parse_variable_segment(clean_segment)
                    if var_infos:
                        active_container.variables.extend(var_infos)
                        
    return containers

def escape_for_mermaid(type_str):
    """Replace < and > with ~ to avoid syntax errors in Mermaid diagrams."""
    return type_str.replace('<', '~').replace('>', '~')

def build_mermaid_class_diagram(all_containers):
    """Generate a Mermaid class diagram string from CSharpContainer structures."""
    lines = ["classDiagram"]
    
    # 1. Assign unique IDs to each parsed container to handle duplicates cleanly
    container_ids = {}
    for i, c in enumerate(all_containers):
        container_ids[c] = f"C{i}"
        
    # 2. Declare classes with their friendly labels
    for c in all_containers:
        cid = container_ids[c]
        lines.append(f"    class {cid}[\"{c.name}\"]")
        if c.kind == 'interface':
            lines.append(f"    <<interface>> {cid}")
        elif c.kind == 'struct':
            lines.append(f"    <<struct>> {cid}")
            
    # 3. Add variables and functions to each class
    for c in all_containers:
        cid = container_ids[c]
        if c.variables or c.functions:
            lines.append(f"    class {cid} {{")
            for var in c.variables:
                t = escape_for_mermaid(var['type'])
                lines.append(f"        {var['access']}{var['name']} : {t}")
            for func in c.functions:
                lines.append(f"        {func['access']}{func['name']}()")
            lines.append("    }")
            
    # 4. Generate relationships
    declared_externals = set()
    for c in all_containers:
        cid = container_ids[c]
        for base in c.bases:
            # Clean base name (e.g. IComparable<Enemy> -> IComparable)
            base_clean = base.split('<')[0]
            
            # Find if base is defined in our parsed containers
            base_container = None
            for other in all_containers:
                if other.name == base or other.name == base_clean:
                    base_container = other
                    break
                    
            if base_container:
                bid = container_ids[base_container]
            else:
                # External class/interface
                bid = base_clean
                if bid not in declared_externals:
                    lines.append(f"    class {bid}[\"{base}\"]")
                    declared_externals.add(bid)
                    
            # Determine line style (dashed for interfaces starting with 'I' and capital letter, solid for class inheritance)
            is_interface = base_clean.startswith('I') and len(base_clean) > 1 and base_clean[1].isupper()
            rel = "<|.." if is_interface else "<|--"
            lines.append(f"    {bid} {rel} {cid}")
            
    return "\n".join(lines)

def generate_report(root_dir, use_mermaid=False):
    """Search for C# files and print class diagram structure or Mermaid diagram."""
    all_containers = []
    file_containers_map = []
    
    for root, dirs, files in os.walk(root_dir):
        cs_files = [f for f in files if f.endswith('.cs')]
        if not cs_files:
            continue
            
        for file_name in sorted(cs_files):
            file_path = os.path.join(root, file_name)
            try:
                with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                    content = f.read()
            except Exception as e:
                continue
                
            containers = parse_csharp_file(content)
            if containers:
                all_containers.extend(containers)
                rel_path = os.path.relpath(file_path, root_dir)
                file_containers_map.append((rel_path, containers))
                
    if not all_containers:
        print("No C# files with classes, structs, or interfaces were found.")
        return
        
    if use_mermaid:
        print(build_mermaid_class_diagram(all_containers))
    else:
        for rel_path, containers in file_containers_map:
            print(f"\nFile: {rel_path}")
            print("=" * 60)
            for container in containers:
                print(f"{container.kind} {container.name}:")
                if container.variables:
                    print("  Variables:")
                    for var in container.variables:
                        print(f"    {var['access']}{var['name']} : {var['type']}")
                if container.functions:
                    print("  Functions:")
                    for func in container.functions:
                        print(f"    {func['access']}{func['name']}()")
                print()

if __name__ == "__main__":
    search_dir = "."
    use_mermaid = False
    
    args = sys.argv[1:]
    if "--mermaid" in args:
        use_mermaid = True
        args.remove("--mermaid")
        
    if len(args) > 0:
        search_dir = args[0]
        
    if not os.path.exists(search_dir):
        print(f"Directory '{search_dir}' does not exist.")
        sys.exit(1)
        
    generate_report(search_dir, use_mermaid)
