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
                if (ChosenElement == null || !(ChosenElement is Cell)) ChosenElement = newChosenElement;
                else
                {
                    var GoFrom = (Cell)ChosenElement;
                    var GoTo = (Cell)newChosenElement;
                    ChosenElement = null;
                    Map.Move(GoFrom, GoTo);
                    View.RedrawCell(new []{GoTo, GoFrom});
                }
            }
        }
    }
}