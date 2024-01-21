using System;
using UnityEngine;

namespace _Scripts.Kitchen
{
    [Serializable]
    public class FoodVisualizersSet
    {
        public FoodVisualizer RawVisualizer;
        public FoodVisualizer CookedVisualizer;
        public FoodVisualizer OvercookedVisualizer;

        public void Hide()
        {
            RawVisualizer?.SetEnabled(false);
            CookedVisualizer?.SetEnabled(false);
            OvercookedVisualizer?.SetEnabled(false);
        }

        public void ShowEmpty()
        {
            Hide();
        }

        public void ShowStatus(Food.FoodStatus status)
        {
            Hide();
            switch (status)
            {
                case Food.FoodStatus.Raw:
                {
                    RawVisualizer?.SetEnabled(true);
                    return;
                }
                case Food.FoodStatus.Cooked:
                {
                    CookedVisualizer?.SetEnabled(true);
                    return;
                }
                case Food.FoodStatus.Overcooked:
                {
                    OvercookedVisualizer?.SetEnabled(true);
                    return;
                }
            }
        }
    }
}