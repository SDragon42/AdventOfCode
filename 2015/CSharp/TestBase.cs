using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    public abstract class TestBase
    {
        protected readonly string rootPath = TestContext.CurrentContext.TestDirectory;
    }
}
