using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace textCut
{
    class FormatMachine
    {
        public char[] Vowels { get; set; }
        public char[] Consonants { get; set; }
        public int Length { get; set; }

        public char[] SpaceSimbols { get; set; }
        public char[] HyphenationSimbols { get; set; }
        public char[] EndOfFileSimbols { get; set; }
        public char[] EndOfLineSimbols { get; set; }

        public char Space { get; set; }
        public char Hyphenation { get; set; }

        private state currentState { get; set; }  
        private state lastState { get; set; }

        private char currentSimbol;
        private char frozenSimbol;

        private StringBuilder ans;

        private string inputString;
        private int inputStringIndex;

        private StreamReader fileR;
        private StreamWriter fileW;

        private rules currentRules;

        private enum state
        {
            Active, Pause, Frozen, End
        }


        public FormatMachine ()
        {
            ans = new StringBuilder();
            Hyphenation = '-';
            Space = ' ';
            EndOfLineSimbols = new char[] { '\n' };
            EndOfFileSimbols = new char[] { (char)65535 };
            HyphenationSimbols = new char[] { '-' };
            SpaceSimbols = new char[] { '\r', '\t', ' ' };
        }

        private void BeginStateInitialization ()
        {
            currentState = state.Pause;
            lastState = state.Active;
            currentRules = new rules(this);
        }


        public void FormatFromFile(StreamReader fileR, StreamWriter fileW)
        {
            this.fileR = fileR;
            this.fileW = fileW;
            Check();
            if (fileR == null)
                throw new FileLoadException("Input file wasn't initialized.");
            this.fileR = fileR;
            BeginStateInitialization();
            while (StateMachine()) ;
            EndDispose();
        }

        public void FormatFromString(string s, StreamWriter fileW)
        {
            this.fileW = fileW;
            Check();
            if (s == null)
                throw new FieldAccessException("Fields or properties were initialized uncorrectly.");
            BeginStateInitialization();
            while (StateMachine());
            EndDispose();
        }


        private bool StateMachine()
        {
            currentSimbol = GetChar();
            if (ans !=null && NeedCut())
            {
                try
                {
                    while (ans.Length >= Length)
                        CutOut(ans, Length);
                    if (ans.Length == 0)
                        currentSimbol = GetChar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (isLetter(EndOfFileSimbols, currentSimbol))
            {
                End();
                return false;
            }
            if (isLetter(EndOfLineSimbols, currentSimbol))
                if (currentState == state.Frozen || currentState == state.Pause && lastState == state.Frozen)
                    ChangeState (state.Active, state.Pause);
                else
                    Pause();
            else
                if (isLetter(SpaceSimbols, currentSimbol))
                    Pause();
                else
                    if (isLetter(HyphenationSimbols, currentSimbol))
                        Froze();
                    else
                        Active();
            return true;
        }

        private void End()
        {
            if (currentState == state.Frozen)
                ans.Append(frozenSimbol);
            while (ans.Length != 0)
                CutOut(ans, Length);
        }

        private void Pause()
        {
            switch (currentState)
            {
                case state.Frozen:
                    ChangeState(state.Frozen, state.Pause);
                    break;
                case state.Active:
                    ChangeState(state.Active, state.Pause, Space);
                    break;
            }
        }

        private void Froze()
        {
            switch (currentState)
            {
                case state.Frozen:
                    ChangeState(state.Active, state.Active, frozenSimbol, currentSimbol);
                    break;
                case state.Pause:
                    if (lastState == state.Frozen)
                        ChangeState(state.Frozen, state.Active, frozenSimbol, Space, currentSimbol);
                    else
                        ChangeState(state.Pause, state.Active, currentSimbol);
                    break;
                default:
                    frozenSimbol = currentSimbol;
                    ChangeState(currentState, state.Frozen);
                    break;
            }
        }

        private void Active()
        {
            switch (currentState)
            {
                case state.Frozen:
                    ChangeState(state.Active, state.Active, frozenSimbol, currentSimbol);
                    break;
                case state.Pause:
                    if (lastState == state.Frozen)
                        ChangeState(state.Frozen, state.Active, frozenSimbol, Space, currentSimbol);
                    else
                        goto label;
                    break;
                default:
                    label:
                    ChangeState(currentState, state.Active, currentSimbol);
                    break;
            }
        }

        private void ChangeState(state last, state current, params char[] add)
        {
            if (add != null)
                for (int i = 0; i < add.Length; i++)
                    ans.Append(add[i]);
            lastState = last;
            currentState = current;
        }


        private void CutOut(StringBuilder ans, int length)
        {
            if (ans.Length <= length)
            {
                fileW.WriteLine(ans);
                ans.Clear();
            }
            else
            {
                int index = 0;
                int wrapPosition = 0;
                for (int i = length - 1; i >= 0; i--)
                    if (isLetter(SpaceSimbols, ans[i]))
                    {
                        index = i;
                        break;
                    }
                int begin = index + 1;
                if (index == 0)
                    begin = 0;
                for (int i = begin; i < length; i++)
                    wrapPosition = max(wrapPosition, currentRules.GetRules(i));
                if (index == 0 && wrapPosition == 0)
                    throw new Exception("Too long word!");
                    for (int i = 0; i <= max(index - 1, wrapPosition); i++)
                    {
                        fileW.Write(ans[0]);
                        ans.Remove(0, 1);
                    }
                    if (wrapPosition != 0)
                        fileW.Write(Hyphenation);
                    fileW.WriteLine();
                    if (isLetter(SpaceSimbols, ans[0]))
                        ans.Remove(0, 1);
            }
        }

        private void Check ()
        {
            if (fileW == null)
                throw new FileLoadException("Output file wasn't initialized.");
            if (Vowels.Length == 0 || Consonants.Length == 0 || Length == 0 || SpaceSimbols.Length == 0 || HyphenationSimbols.Length == 0
                || EndOfLineSimbols.Length == 0 || EndOfFileSimbols.Length == 0)
                throw new FieldAccessException("Fields or properties were initialized uncorrectly.");
        }

        private char GetChar()
        {
            if (inputString != null)
                return inputString[inputStringIndex++];
            else
                return (char)fileR.Read();
        }

        private bool isLetter(char[] letters, params char[] simbols)
        {
            for (int j = 0; j < simbols.Length; j++)
            {
                bool fl = false;
                for (int i = 0; i < letters.Length; i++)
                    if (simbols[j] == letters[i])
                    {
                        fl = true;
                        break;
                    }
                if (!fl)
                    return false;
            }
            return true;
        }

        private bool NeedCut()
        {
            return ans.Length >= Length && isLetter(SpaceSimbols, currentSimbol) && currentState != state.Frozen
                || ans.Length - 2 >= Length && currentState != state.Pause && lastState == state.Frozen;
        }

        private void EndDispose()
        {
            fileR = null;
            fileW = null;
            inputString = null;
            inputStringIndex = 0;
            currentRules = null;
            ans.Clear();
        }

        private int max (int x, int y)
        {
            if (x >= y)
                return x;
            return y;
        }


        private class rules 
        {
            private FormatMachine Parent { get; set; }   //  think about to add space before cutting
            private int currentPosition;
            public rules (FormatMachine parent)
            {
                Parent = parent;
            }
            public int GetRules (int beginPosition)
            {
                currentPosition = beginPosition;
                int position = 0;
                position = Parent.max(position, rule1());
                position = Parent.max(position, rule2());
                return position;
            }
            private int rule1 ()
            {
                if (currentPosition < Parent.Length - 1 && currentPosition < Parent.ans.Length - 3 &&
                    Parent.isLetter(Parent.Consonants, Parent.ans[currentPosition], Parent.ans[currentPosition + 3])
                    && Parent.isLetter(Parent.Vowels, Parent.ans[currentPosition + 1], Parent.ans[currentPosition + 2]))
                    return currentPosition + 1;
                return 0;
            }
            private int rule2 ()
            {
                if (currentPosition < Parent.Length - 1 && 
                    currentPosition < Parent.ans.Length - 2 && 
                    Parent.isLetter(Parent.Consonants, Parent.ans[currentPosition], Parent.ans[currentPosition + 2]) &&
                    Parent.isLetter(Parent.Vowels, Parent.ans[currentPosition + 1]))
                    return currentPosition + 1;
                return 0;
            }
        }
    }
}
