using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimulationLib {
    public interface IDraw {
        void Draw();
        ImageSource ImageSource { get; set; }
    }
}
