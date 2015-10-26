﻿using System;
using _3PA.Lib;
using _3PA.MainFeatures.AutoCompletion;

namespace _3PA.MainFeatures.InfoToolTip {
    class InfoToolTip {

        private static InfoToolTipForm _form;

        /// <summary>
        /// Was the form opened because the user left his mouse too long on a word?
        /// </summary>
        private static bool _openedFromDwell;

        public static void ShowToolTip(bool openedFromDwell = false) {
            if (Config.Instance.ToolTipDeactivate) return;

            // remember if the popup was opened because of the dwell time
            _openedFromDwell = openedFromDwell;

            // instanciate the form
            if (_form == null) {
                _form = new InfoToolTipForm {
                    UnfocusedOpacity = Config.Instance.ToolTipUnfocusedOpacity,
                    FocusedOpacity = Config.Instance.ToolTipFocusedOpacity
                };
                _form.Show(Npp.Win32WindowNpp);
            }

            // opened from dwell
            if (openedFromDwell) {
                var position = Npp.GetPositionFromMouseLocation();
                
                // sets the tooltip content
                if (!SetToolTip(position)) return;

                // update position
                var point = Npp.GetPointXyFromPosition(position);
                point.Offset(Npp.GetWindowRect().Location);
                var lineHeight = Npp.GetTextHeight(Npp.GetCaretLineNumber());
                point.Y += lineHeight + 5;
                _form.SetPosition(point, lineHeight + 5);
            }

            if (!_form.Visible)
                _form.UnCloack();
        }

        /// <summary>
        /// Sets the content of the tooltip (when we want to descibe something present
        /// in the completionData list)
        /// </summary>
        /// <param name="position"></param>
        private static bool SetToolTip(int position) {
            if (position < 0) return false;

            // retrieves the corresponding completionData
            var data = AutoComplete.FindInCompletionData(Npp.GetWordAtPosition(position), position);
            if (data == null) return false;

            switch (data.Type) {
                case CompletionType.Database:

                    break;
            }

            _form.SetText("<div class='InfoToolTip'><b>THIS ISSSS</b><br>A simple test :)<br><img src='wink'>hey<br>" + data.DisplayText + "</div>");

            return true;
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        public static void Close(bool calledFromDwellEnd = false) {
            try {
                if (calledFromDwellEnd && !_openedFromDwell) return;
                _form.Cloack();
                _openedFromDwell = false;
            } catch (Exception) {
                // ignored
            }
        }

        /// <summary>
        /// Forces the form to close, only when leaving npp
        /// </summary>
        public static void ForceClose() {
            try {
                _form.ForceClose();
                _form = null;
            } catch (Exception) {
                // ignored
            }
        }
    }
}
