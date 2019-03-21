﻿using System.Linq;
using Snapper.Core;
using Snapper.Core.TestMethodResolver;

namespace Snapper.Nunit
{
    internal class NUnitEnvironmentVariableUpdateDecider : ISnapshotUpdateDecider
    {
        private readonly ISnapshotUpdateDecider _envUpdateDecider;

        public NUnitEnvironmentVariableUpdateDecider()
        {
            _envUpdateDecider = new SnapshotUpdateDecider(new TestMethodResolver());
        }

        public bool ShouldUpdateSnapshot()
        {
            if (_envUpdateDecider.ShouldUpdateSnapshot())
                return true;

            var (method, _) = NUnitTestHelper.GetCallingTestInfo();

            var methodHasAttribute = method?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;
            var classHasAttribute =
                method?.ReflectedType?.GetCustomAttributes(typeof(UpdateSnapshots), true).Any() ?? false;

            return methodHasAttribute || classHasAttribute;
        }
    }
}