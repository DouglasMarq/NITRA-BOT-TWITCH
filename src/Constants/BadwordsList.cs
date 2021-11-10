using System.Collections.Generic;

namespace NITRA_BOT_TWITCH
{
    public class BadwordsList
    {
        private readonly List<string> _badwords = new List<string>{ "fuck", "test" };

        public List<string> Badwords {
            get => _badwords;
        }
    }
}
