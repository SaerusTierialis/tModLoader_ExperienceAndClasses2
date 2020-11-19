using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Containers
{
    public abstract class AttributeContainer
    {
        private AutoDataPlayer<int> _allocated;
        private AutoDataPlayer<int> _bonus;
        
        public int Allocated { get { return _allocated.value; } }
        public int Bonus { get { return _bonus.value; } set { _bonus.value = value; } }

        public int Final { get; private set; }

        public string Name { get; protected set; } = "Default";
        public string Tooltip { get; protected set; } = "Default";

        public AttributeContainer(AutoDataPlayer<int> allocated, AutoDataPlayer<int> bonus)
        {
            _allocated = allocated;
            _bonus = bonus;
        }

        public void PostUpdate(PlayerData player_data)
        {
            CalculateFinal();
            DoEffect(player_data, Final);
        }

        private void CalculateFinal()
        {
            Final = Allocated + Bonus;
        }

        protected abstract void DoEffect(PlayerData player_data, int value);
    }
}
