using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segment_tree {

    public class SumSegmentTree: IEnumerable {
        int[] tree;
        int size;
        
        public SumSegmentTree(IEnumerable<int> arr) {
            tree = new int[arr.Count() * 4 + 1];
            size = arr.Count();
            build(arr.ToArray(), 0, 0, size);
        }

        private void build(int[] arr, int v, int l, int r) {
            if (l == r - 1) {
                tree[v] = arr[l];
                return;
            }
            int m = (l + r) / 2;
            build(arr, v * 2 + 1, l, m);
            build(arr, v * 2 + 2, m, r);
            tree[v] = tree[2 * v + 1] + tree[2 * v + 2];
        }

        private void update(int v, int l, int r, int pos, int val) {
            if (l == r - 1)
                tree[v] = val;
            else {
                int m = (l + r) / 2;
                if (pos < m)
                    update(2 * v + 1, l, m, pos, val);
                else
                    update(2 * v + 2, m, r, pos, val);
                tree[v] = tree[2 * v + 1] + tree[2 * v + 2];
            }
        }

        public int GetSum(int l, int r) {
            if (l > r || l < 0 || r >= size)
                throw new IndexOutOfRangeException("Invalid interval");
            return GetSum(0, 0, size, l, r + 1);
        }

        private int GetSum(int v, int tl, int tr, int l, int r) {
            if (l <= tl && r >= tr)
                return tree[v];
            if (tr <= l || tl >= r)
                return 0;
            int tm = (tl + tr) / 2;
            return GetSum(2 * v + 1, tl, tm, l, r) + GetSum(2 * v + 2, tm, tr, l, r);
        }

        public IEnumerator GetEnumerator() {
            for (int i = 0; i < size; i++) {
                yield return this[i];
            }
        }

        public int this[int i] {
            get {
                if (i < 0 || i >= size)
                    throw new IndexOutOfRangeException("The index is smaller or bigger than array's size");
                return GetSum(0, 0, size, i, i + 1);
            }
            set {
                if (i < 0 || i >= size)
                    throw new IndexOutOfRangeException("The index is smaller or bigger than array's size");
                update(0, 0, size, i, value);
            }
        }
    }
    class Program {
        static void Main(string[] args) {
            try {
                Console.WriteLine("Set an array; Available operations: arr[i], arr.GetSum(int l, int r); foreach");
                do {

                    int[] s = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();

                    SumSegmentTree tree = new SumSegmentTree(s);

                    Console.WriteLine(tree[2]);

                    Console.WriteLine(tree[0]);

                    tree[2] = 3;

                    Console.WriteLine(tree[4]);

                    Console.WriteLine(tree[2]);

                    Console.WriteLine(tree.GetSum(1, 3));

                    foreach (var el in tree)
                        Console.Write(el + " ");
                    Console.WriteLine();

                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            } catch (IndexOutOfRangeException ex) {
                Console.WriteLine(ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Something wrong: \n" + ex.Message);
            }
        }
    }
}
