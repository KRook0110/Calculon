mergeInto(LibraryManager.library, {
  ReactMessage: function (message) {
    window.dispatchReactUnityEvent("ReactMessage", UTF8ToString(message));
  },
});