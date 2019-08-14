﻿using System;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Collections.Methods.Trees
{
    public abstract class SizeBalancedTreeMethods<TElement> : SizedBinaryTreeMethodsBase<TElement>
    {
        protected override void AttachCore(IntPtr root, TElement node)
        {
            while (true)
            {
                var left = GetLeftPointer(root.GetValue<TElement>());
                var leftSize = GetSizeOrZero(left.GetValue<TElement>());
                var right = GetRightPointer(root.GetValue<TElement>());
                var rightSize = GetSizeOrZero(right.GetValue<TElement>());
                if (FirstIsToTheLeftOfSecond(node, root.GetValue<TElement>())) // node.Key less than root.Key
                {
                    if (EqualToZero(left.GetValue<TElement>()))
                    {
                        IncrementSize(root.GetValue<TElement>());
                        SetSize(node, GetOne());
                        left.SetValue(node);
                        break;
                    }
                    if (FirstIsToTheRightOfSecond(node, left.GetValue<TElement>())) // node.Key greater than left.Key
                    {
                        var leftRight = GetRightValue(left.GetValue<TElement>());
                        var leftRightSize = GetSizeOrZero(leftRight);
                        if (GreaterThan(Increment(leftRightSize), rightSize))
                        {
                            if (EqualToZero(leftRightSize) && EqualToZero(rightSize))
                            {
                                SetLeft(node, left.GetValue<TElement>());
                                SetRight(node, root.GetValue<TElement>());
                                SetSize(node, Add(GetSize(left.GetValue<TElement>()), GetTwo())); // Two (2) - размер ветки *root (right) и самого node
                                SetLeft(root.GetValue<TElement>(), GetZero());
                                SetSize(root.GetValue<TElement>(), GetOne());
                                root.SetValue(node);
                                break;
                            }
                            LeftRotate(left);
                            RightRotate(root);
                        }
                        else
                        {
                            IncrementSize(root.GetValue<TElement>());
                            root = left;
                        }
                    }
                    else // node.Key less than left.Key
                    {
                        var leftLeft = GetLeftValue(left.GetValue<TElement>());
                        var leftLeftSize = GetSizeOrZero(leftLeft);
                        if (GreaterThan(Increment(leftLeftSize), rightSize))
                        {
                            RightRotate(root);
                        }
                        else
                        {
                            IncrementSize(root.GetValue<TElement>());
                            root = left;
                        }
                    }
                }
                else // node.Key greater than root.Key
                {
                    if (EqualToZero(right.GetValue<TElement>()))
                    {
                        IncrementSize(root.GetValue<TElement>());
                        SetSize(node, GetOne());
                        right.SetValue(node);
                        break;
                    }
                    if (FirstIsToTheRightOfSecond(node, right.GetValue<TElement>())) // node.Key greater than right.Key
                    {
                        var rightRight = GetRightValue(right.GetValue<TElement>());
                        var rightRightSize = GetSizeOrZero(rightRight);
                        if (GreaterThan(Increment(rightRightSize), leftSize))
                        {
                            LeftRotate(root);
                        }
                        else
                        {
                            IncrementSize(root.GetValue<TElement>());
                            root = right;
                        }
                    }
                    else // node.Key less than right.Key
                    {
                        var rightLeft = GetLeftValue(right.GetValue<TElement>());
                        var rightLeftSize = GetSizeOrZero(rightLeft);
                        if (GreaterThan(Increment(rightLeftSize), leftSize))
                        {
                            if (EqualToZero(rightLeftSize) && EqualToZero(leftSize))
                            {
                                SetLeft(node, root.GetValue<TElement>());
                                SetRight(node, right.GetValue<TElement>());
                                SetSize(node, Add(GetSize(right.GetValue<TElement>()), GetTwo())); // Two (2) - размер ветки *root (left) и самого node
                                SetRight(root.GetValue<TElement>(), GetZero());
                                SetSize(root.GetValue<TElement>(), GetOne());
                                root.SetValue(node);
                                break;
                            }
                            RightRotate(right);
                            LeftRotate(root);
                        }
                        else
                        {
                            IncrementSize(root.GetValue<TElement>());
                            root = right;
                        }
                    }
                }
            }
        }

        protected override void DetachCore(IntPtr root, TElement node)
        {
            while (true)
            {
                var left = GetLeftPointer(root.GetValue<TElement>());
                var leftSize = GetSizeOrZero(left.GetValue<TElement>());
                var right = GetRightPointer(root.GetValue<TElement>());
                var rightSize = GetSizeOrZero(right.GetValue<TElement>());
                if (FirstIsToTheLeftOfSecond(node, root.GetValue<TElement>())) // node.Key less than root.Key
                {
                    EnsureNodeInTheTree(node, left);
                    var rightLeft = GetLeftValue(right.GetValue<TElement>());
                    var rightLeftSize = GetSizeOrZero(rightLeft);
                    var rightRight = GetRightValue(right.GetValue<TElement>());
                    var rightRightSize = GetSizeOrZero(rightRight);
                    if (GreaterThan(rightRightSize, Decrement(leftSize)))
                    {
                        LeftRotate(root);
                    }
                    else if (GreaterThan(rightLeftSize, Decrement(leftSize)))
                    {
                        RightRotate(right);
                        LeftRotate(root);
                    }
                    else
                    {
                        DecrementSize(root.GetValue<TElement>());
                        root = left;
                    }
                }
                else if (FirstIsToTheRightOfSecond(node, root.GetValue<TElement>())) // node.Key greater than root.Key
                {
                    EnsureNodeInTheTree(node, right);
                    var leftLeft = GetLeftValue(left.GetValue<TElement>());
                    var leftLeftSize = GetSizeOrZero(leftLeft);
                    var leftRight = GetRightValue(left.GetValue<TElement>());
                    var leftRightSize = GetSizeOrZero(leftRight);
                    if (GreaterThan(leftLeftSize, Decrement(rightSize)))
                    {
                        RightRotate(root);
                    }
                    else if (GreaterThan(leftRightSize, Decrement(rightSize)))
                    {
                        LeftRotate(left);
                        RightRotate(root);
                    }
                    else
                    {
                        DecrementSize(root.GetValue<TElement>());
                        root = right;
                    }
                }
                else // key equals to root.Key
                {
                    if (GreaterThanZero(leftSize) && GreaterThanZero(rightSize))
                    {
                        if (GreaterThan(leftSize, rightSize))
                        {
                            var replacement = left.GetValue<TElement>();
                            while (!EqualToZero(GetRightValue(replacement)))
                            {
                                replacement = GetRightValue(replacement);
                            }
                            DetachCore(left, replacement);
                            SetLeft(replacement, left.GetValue<TElement>());
                            SetRight(replacement, right.GetValue<TElement>());
                            FixSize(replacement);
                            root.SetValue(replacement);
                        }
                        else
                        {
                            var replacement = right.GetValue<TElement>();
                            while (!EqualToZero(GetLeftValue(replacement)))
                            {
                                replacement = GetLeftValue(replacement);
                            }
                            DetachCore(right, replacement);
                            SetLeft(replacement, left.GetValue<TElement>());
                            SetRight(replacement, right.GetValue<TElement>());
                            FixSize(replacement);
                            root.SetValue(replacement);
                        }
                    }
                    else if (GreaterThanZero(leftSize))
                    {
                        root.SetValue(left.GetValue<TElement>());
                    }
                    else if (GreaterThanZero(rightSize))
                    {
                        root.SetValue(right.GetValue<TElement>());
                    }
                    else
                    {
                        root.SetValue(GetZero());
                    }
                    ClearNode(node);
                    break;
                }
            }
        }

        private void EnsureNodeInTheTree(TElement node, IntPtr branch)
        {
            if (EqualToZero(branch.GetValue<TElement>()))
            {
                throw new InvalidOperationException($"Элемент {node} не содержится в дереве.");
            }
        }
    }
}