using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace bc
{
    public class Calculatrix
    {
        private Queue<CharacterActions> final;
        private Stack<CharacterActions> operators;
        private CharacterActions CurrentChar;

        public Calculatrix()
        {
            InitializeExpressions();
        }

        public void Parse(string Expression)
        {
            string cleanedUpInput = CleanupInput(Expression);

            GetPostfixVersion(cleanedUpInput);
        }

        public double Calculate(string expression)
        {
            Parse(expression);

            Stack<double> finalOutput = new Stack<double>();
            foreach (var item in final)
            {
                CurrentChar = item;
                CurrentChar.CharacterFunc(finalOutput);
            }

            if (finalOutput.Count == 1)
            {
                double result = Math.Floor(finalOutput.Pop());
                return result;
            }
            else
            {
                throw new Exception();
            }
        }
        #region postfix ops
        private void DivideOp(Stack<double> result)
        {
            if (result.Count >= 2)
            {
                double first = 0.0, second = 0.0;
                second = result.Pop();
                first = result.Pop();
                result.Push(first / second);
            }
            else
            {
                throw new Exception();
            }
        }

        private void MultiplyOp(Stack<double> result)
        {
            if (result.Count >= 2)
            {
                double first = 0.0, second = 0.0;
                second = (double)result.Pop();
                first = (double)result.Pop();
                result.Push(first * second);
            }
            else
            {
                throw new Exception();
            }
        }

        private void MinusOp(Stack<double> result)
        {
            if (result.Count >= 2)
            {
                double first = 0.0, second = 0.0;
                second = (double)result.Pop();
                first = (double)result.Pop();
                result.Push(first - second);
            }
            else
            {
                throw new Exception();
            }
        }

        private void PlusOp(Stack<double> result)
        {
            if (result.Count >= 2)
            {
                double first = 0.0, second = 0.0;
                second = (double)result.Pop();
                first = (double)result.Pop();
                result.Push(first + second);
            }
            else
            {
                throw new Exception();
            }
        }

        private void NumberOp(Stack<double> result)
        {
            result.Push(double.Parse(CurrentChar.CharacterValue));
        }
        #endregion
        private bool IsOperatorToken(CharCategory t)
        {
            bool result = false;
            switch (t)
            {
                case CharCategory.Add:
                case CharCategory.Subtract:
                case CharCategory.Multiply:
                case CharCategory.Divide:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        private void InitializeExpressions()
        {
            final = new Queue<CharacterActions>();
            operators = new Stack<CharacterActions>();
            CurrentChar = new CharacterActions();

        }
        private string CleanupInput(string Expression)
        {
            string final = Expression;
            final = Regex.Replace(final, @"(?<num>\d+(\.\d+)?)", " ${num} ");
            final = Regex.Replace(final, @"(?<operator>[+\-*/])", " ${operator} ");
            final = Regex.Replace(final, @"\s+", " ").Trim();
            return final;
        }
        private void GetPostfixVersion(string sBuffer)
        {
            string[] chars = sBuffer.Split(" ".ToCharArray());
            double charValue;
            CharacterActions character;
            CharacterActions actionCharacter;
            foreach (var item in chars)
            {
                character = new CharacterActions();
                character.CharacterValue = item;
                character.CharacterCategory = CharCategory.NA;

                try
                {
                    charValue = double.Parse(item);
                    character.CharacterCategory = CharCategory.Num;
                    character.CharacterFunc = NumberOp;
                    final.Enqueue(character);
                }
                catch
                {
                    switch (item)
                    {
                        case "+":
                            character.CharacterCategory = CharCategory.Add;
                            character.CharacterFunc = PlusOp;
                            if (operators.Count > 0)
                            {
                                actionCharacter = operators.Peek();
                                while (IsOperatorToken(actionCharacter.CharacterCategory))
                                {
                                    final.Enqueue(operators.Pop());
                                    if (operators.Count > 0)
                                    {
                                        actionCharacter = operators.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            operators.Push(character);
                            break;
                        case "-":
                            character.CharacterCategory = CharCategory.Subtract;
                            character.CharacterFunc = MinusOp;
                            if (operators.Count > 0)
                            {
                                actionCharacter = operators.Peek();
                                while (IsOperatorToken(actionCharacter.CharacterCategory))
                                {
                                    final.Enqueue(operators.Pop());
                                    if (operators.Count > 0)
                                    {
                                        actionCharacter = operators.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            operators.Push(character);
                            break;
                        case "*":
                            character.CharacterCategory = CharCategory.Multiply;
                            character.CharacterFunc = MultiplyOp;
                            if (operators.Count > 0)
                            {
                                actionCharacter = operators.Peek();
                                while (IsOperatorToken(actionCharacter.CharacterCategory))
                                {
                                    if (actionCharacter.CharacterCategory == CharCategory.Add || actionCharacter.CharacterCategory == CharCategory.Subtract)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        final.Enqueue(operators.Pop());
                                        if (operators.Count > 0)
                                        {
                                            actionCharacter = operators.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            operators.Push(character);
                            break;
                        case "/":
                            character.CharacterCategory = CharCategory.Divide;
                            character.CharacterFunc = DivideOp;
                            if (operators.Count > 0)
                            {
                                actionCharacter = (CharacterActions)operators.Peek();
                                while (IsOperatorToken(actionCharacter.CharacterCategory))
                                {
                                    if (actionCharacter.CharacterCategory == CharCategory.Add || actionCharacter.CharacterCategory == CharCategory.Subtract)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        final.Enqueue(operators.Pop());
                                        if (operators.Count > 0)
                                        {
                                            actionCharacter = operators.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            operators.Push(character);
                            break;
                    }
                }
            }


            while (operators.Count != 0)
            {
                actionCharacter = operators.Pop();
                final.Enqueue(actionCharacter);
            }
        }
    }
}