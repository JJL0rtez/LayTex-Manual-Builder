using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LayTexFileCreator
{
    public class Config
    {
        // Config Values

        public Brush BACKGROUND_COLOR { get; }
        public Brush CONTROL_COLOR { get; }
        public Brush GUI_COLOR { get; }
        public Brush ACCENT_COLOR { get; }

        public Config() {
            BACKGROUND_COLOR = Brushes.AntiqueWhite;
            CONTROL_COLOR = Brushes.AntiqueWhite;
            GUI_COLOR = Brushes.FloralWhite;
            ACCENT_COLOR = Brushes.LightGray;
         }
    }
}
