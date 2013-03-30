using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings.Infrastructure;

namespace TheSettingsTests.TreeTests
{
    [TestClass]
    public class TreeWalkerTests
    {
        private ITree<TreeNode> _tree;
        private ITreeWalker<TreeNode> _treeWalker;

        [TestInitialize]
        public void Initialize()
        {
            _tree = new Tree(6);
            _treeWalker = new TreeWalker<TreeNode>(16, _tree);
        }

        [TestMethod]
        public void ShouldStoreInitialCurrentValueToCurrent()
        {
            AssertCurrentItemIs(16);
        }

        [TestMethod]
        public void ShouldMoveToParentWhenClimbingUpOnce()
        {
            _treeWalker.ClimbUp(1);
            AssertCurrentItemIs(8);
        }

        [TestMethod]
        public void ShouldClimbUpToGrandParentWhenClimbing()
        {
            _treeWalker.ClimbUp(2);
            AssertCurrentItemIs(4);
        }

        [TestMethod]
        public void ShouldClimbDownToFirstChildWhenClimbingDown()
        {
            _treeWalker.ClimbDown(0);
            AssertCurrentItemIs(32);
        }

        [TestMethod]
        public void ShouldClimbDownToSecondGrandChildOfSecondChild()
        {
            _treeWalker.ClimbDown(1, 1);
            AssertCurrentItemIs(67);
        }

        [TestMethod]
        public void ShouldClimbUpToRoot()
        {
            _treeWalker.ClimbToRoot();
            AssertCurrentItemIs(1);
        }

        [TestMethod]
        public void ShouldGetDepthFirstDownards()
        {
            var actual = _treeWalker.GetDepthFirstDownards(16).Select(i => i.Value).ToList();
            CollectionAssert.AreEqual(new List<int> { 32, 64, 65, 33, 66, 67 }, actual);
        }

        [TestMethod]
        public void ShouldGetDepthFirstUpwards()
        {
            _treeWalker = new TreeWalker<TreeNode>(5, new Tree(3));
            var actual = _treeWalker.GetDepthFirstUpwards(5).Select(i => i.Value).ToList();
            CollectionAssert.AreEqual(
                new List<int> { 
                    4, 8, 9, 2, 3, 6, 12, 13, 7, 14, 15, 1
                }, actual, string.Join(", ", actual));
        }

        private void AssertCurrentItemIs(int expected)
        {
            Assert.AreEqual(expected, _treeWalker.Current.Value);
        }

        private class TreeNode
        {
            public TreeNode(int value)
            {
                Value = value;
            }

            public static implicit operator TreeNode(int value)
            {
                return new TreeNode(value);
            }

            public static implicit operator int(TreeNode item)
            {
                return item.Value;
            }

            public int Value { get; private set; }

            public override string ToString()
            {
                return Value.ToString();
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                var other = obj as TreeNode;
                return other != null && other.Value == Value;
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        private class Tree : ITree<TreeNode>
        {
            private const int _nodeChildCount = 2;
            private int _maximumNodeValue;

            public Tree(int nodeLevels)
            {
                _maximumNodeValue = (int)Math.Pow(_nodeChildCount, nodeLevels) - 1;
            }

            public TreeNode GetParent(TreeNode element)
            {
                if (element < _nodeChildCount)
                {
                    return null;
                }
                return element / _nodeChildCount;
            }

            public TreeNode GetChild(TreeNode element, int index)
            {
                return element * _nodeChildCount + index;
            }

            public IEnumerable<TreeNode> GetChildren(TreeNode element)
            {
                return element > _maximumNodeValue
                    ? Enumerable.Empty<TreeNode>()
                    : Enumerable.Range(element * _nodeChildCount, _nodeChildCount).Select(v => new TreeNode(v));
            }

            public int GetChildrenCount(TreeNode element)
            {
                return element > _maximumNodeValue
                    ? 0
                    : _nodeChildCount;
            }
        }
    }
}
