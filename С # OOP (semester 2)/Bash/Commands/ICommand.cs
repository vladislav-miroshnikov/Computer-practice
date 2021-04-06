using System.Collections.Generic;

namespace Commands
{
    public interface ICommand
    {
        List<string> Arguments { get; set; }
        List<string> Result { get; set; }
        bool IsFirstCommand { get; set; }

        void Execute();

        bool IsCorrectArgs();
    }
}
