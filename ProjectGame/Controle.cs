using System.Runtime.CompilerServices;

namespace ProjectGame
{
    public static class Controle
    {
        private static object ChosenElement;
        public static Map Map;

        public static void RecieveSignal(object newChosenElement)
        {
            if (newChosenElement is Cell)
            {
                var GoTo = (Cell)newChosenElement;
                if (ChosenElement != null && ChosenElement is Cell)
                {
                    var GoFrom = (Cell)ChosenElement;
                    Map.Move(GoFrom, GoTo);
                    GoFrom.UnSetChosen();
                }
                ChosenElement = newChosenElement;
                GoTo.SetChosen();
            }
        }
    }
}