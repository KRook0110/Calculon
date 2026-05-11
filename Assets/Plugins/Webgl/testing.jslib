mergeInto(LibraryManager.library, {
  ReactMessage: function (message, number) {
    console.log("Testing From React Message");
    window.dispatchReactUnityEvent("ReactMessage", UTF8ToString(message), number);
  },
});