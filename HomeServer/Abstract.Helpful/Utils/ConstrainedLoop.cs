using System;

namespace Abstract.Helpful.Lib.Utils
{
    public sealed class ConstrainedLoop
    {
        private const long MAX_LOOP_COUNT = 10000;

        public static void While(Func<bool> predicate, Func<LoopActionType> action, string actionText,
            long maxLoopCount = MAX_LOOP_COUNT)
        {
            var loopCount = 0;
            while (loopCount < maxLoopCount && predicate())
            {
                var loopExitType = action();
                loopCount++;
                if (loopExitType == LoopActionType.Break)
                    break;
            }
            if (loopCount >= maxLoopCount)
                throw new LoopException($"Max Loop Amount Reached {maxLoopCount} on action: '{actionText}'");
        }
    }
}