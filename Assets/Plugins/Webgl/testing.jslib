mergeInto(LibraryManager.library, {
  ReactMessage: function (message, number) {
    window.dispatchReactUnityEvent("ReactMessage", UTF8ToString(message), number);
  },
});