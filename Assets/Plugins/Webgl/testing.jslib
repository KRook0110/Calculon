mergeInto(LibraryManager.library, {
  Init: function () {
    window.dispatchReactUnityEvent("Init");
  },

  Level: function (level_name) {
    window.dispatchReactUnityEvent("Level", UTF8ToString(level_name));
  },

  Answer: function (correct) {
    window.dispatchReactUnityEvent("Answer", correct);
  },

  Finished: function (alive) {
    window.dispatchReactUnityEvent("Finished", alive);
  },

  ReactMessage: function (message) {
    window.dispatchReactUnityEvent("ReactMessage", UTF8ToString(message));
  },
});
