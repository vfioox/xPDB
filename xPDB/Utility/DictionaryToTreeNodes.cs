using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xPDB.Utility
{
    using Doo = Dictionary<object, object>;
    class DictionaryToTreeNodes
    {
        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
        {
            return table
              .Cast<DictionaryEntry>()
              .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
        }
        public List<TreeNode> dictToTree(object input)
        {
            return getNodes(input);
        }
        public List<TreeNode> getNodesArray(object input)
        {
            ArrayList operation = new ArrayList();
            if (input.GetType() == typeof(ArrayList))
            {
                operation = (ArrayList)input;
            }
            else if (input.GetType() == typeof(System.Object[]))
            {
                operation = new ArrayList((System.Object[])input);
            }
            else if (input.GetType() == typeof(Array))
            {
                operation = new ArrayList((Array)input);
            }
            else
            {
                throw new Exception("Invalid type");
            }
            List<TreeNode> d = new List<TreeNode>();
            int dictcount = 0;
            int arraycount = 0;
            foreach (object first in (ArrayList)operation)
            {
                TreeNode s = new TreeNode();
                if (first == null)
                {
                    s = new TreeNode("null");
                    s.ImageIndex = 5;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(string))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 3;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(int))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 10;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(double))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 8;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(long))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 11;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(float))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 9;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(bool))
                {
                    s = new TreeNode(first.ToString());
                    s.ImageIndex = 1;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(Image))
                {
                    s = new TreeNode("Image");
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(Color))
                {
                    s = new TreeNode(((Color)first).ToArgb().ToString());
                    s.BackColor = (Color)first;
                    s.ImageIndex = 13;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(ArrayList))
                {
                    List<TreeNode> h = new List<TreeNode>();
                    foreach (TreeNode w in getNodesArray((ArrayList)first))
                    {
                        h.Add(w);
                    }
                    TreeNode[] z = h.ToArray();
                    s = new TreeNode("[" + arraycount.ToString() + "](" + ((ArrayList)first).Count.ToString() + ")", z);
                    arraycount++;
                    s.ImageIndex = 0;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(System.Object[]))
                {
                    List<TreeNode> h = new List<TreeNode>();
                    foreach (TreeNode w in getNodesArray((System.Object[])first))
                    {
                        h.Add(w);
                    }
                    TreeNode[] z = h.ToArray();
                    s = new TreeNode("[" + arraycount.ToString() + "](" + ((System.Object[])first).Count().ToString() + ")", z);
                    arraycount++;
                    s.ImageIndex = 0;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(Doo))
                {
                    List<TreeNode> h = new List<TreeNode>();
                    foreach (TreeNode w in getNodes((Doo)first))
                    {
                        h.Add(w);
                    }
                    TreeNode[] z = h.ToArray();
                    s = new TreeNode("{" + dictcount.ToString() + "}(" + ((Doo)first).Keys.Count().ToString() + ")", z);
                    dictcount++;
                    s.ImageIndex = 14;
                    d.Add(s);
                    continue;
                }
                if (first.GetType() == typeof(Hashtable))
                {
                    List<TreeNode> h = new List<TreeNode>();
                    foreach (TreeNode w in getNodes((Hashtable)first))
                    {
                        h.Add(w);
                    }
                    TreeNode[] z = h.ToArray();
                    s = new TreeNode("{" + dictcount.ToString() + "}(" + ((Hashtable)first).Keys.Count.ToString() + ")", z);
                    dictcount++;
                    s.ImageIndex = 15;
                    d.Add(s);
                    continue;
                }
                s = new TreeNode("obj?" + first.ToString());
                s.ImageIndex = 2;
                d.Add(s);
            }
            return d;
        }
        public List<TreeNode> getNodes(object input)
        {
            Doo operation = new Doo();
            if (input.GetType() == typeof(Doo))
            {
                operation = (Doo)input;
            }
            else if (input.GetType() == typeof(Hashtable))
            {
                operation = HashtableToDictionary<object, object>((Hashtable)input);
            }
            else
            {
                throw new Exception("Invalid type");
            }
            List<TreeNode> d = new List<TreeNode>();
            foreach (KeyValuePair<object, object> first in (Doo)operation)
            {
                if (first.Key.GetType() == typeof(string))
                {
                    TreeNode s = new TreeNode(first.Key.ToString());
                    if (first.Value == null)
                    {
                        s = new TreeNode(first.Key.ToString() + "  :  null");
                        s.ImageIndex = 5;
                        d.Add(s);
                        continue;
                    }
                    else
                    {
                        if (first.Value.GetType() == typeof(string))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  \"" + first.Value.ToString() + "\"");
                            s.ImageIndex = 3;
                            s.ForeColor = Color.Maroon;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(int))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + first.Value.ToString());
                            s.ImageIndex = 10;
                            s.ForeColor = Color.Blue;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(double))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + first.Value.ToString());
                            s.ImageIndex = 8;
                            s.ForeColor = Color.BlueViolet;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(long))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + first.Value.ToString());
                            s.ImageIndex = 11;
                            s.ForeColor = Color.DarkCyan;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(float))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + first.Value.ToString());
                            s.ImageIndex = 9;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(bool))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + first.Value.ToString());
                            s.ImageIndex = 1;
                            s.ForeColor = Color.Green;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(Color))
                        {
                            s = new TreeNode(first.Key.ToString() + "  :  " + ((Color)first.Value).ToArgb().ToString());
                            s.BackColor = (Color)first.Value;
                            s.ImageIndex = 1;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(System.Object[]))
                        {
                            List<TreeNode> h = new List<TreeNode>();
                            foreach (TreeNode w in getNodesArray((System.Object[])first.Value))
                            {
                                h.Add(w);
                            }
                            TreeNode[] z = h.ToArray();
                            s = new TreeNode(first.Key.ToString() + " (" + ((System.Object[])first.Value).Count().ToString() + ")", z);
                            s.ImageIndex = 12;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(ArrayList))
                        {
                            List<TreeNode> h = new List<TreeNode>();
                            foreach (TreeNode w in getNodesArray((ArrayList)first.Value))
                            {
                                h.Add(w);
                            }
                            TreeNode[] z = h.ToArray();
                            s = new TreeNode(first.Key.ToString() + " (" + ((ArrayList)first.Value).Count.ToString() + ")", z);
                            s.ImageIndex = 7;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(Doo))
                        {
                            List<TreeNode> h = new List<TreeNode>();
                            foreach (TreeNode w in getNodes((Doo)first.Value))
                            {
                                h.Add(w);
                            }
                            TreeNode[] z = h.ToArray();
                            s = new TreeNode(first.Key.ToString() + " (" + ((Doo)first.Value).Keys.Count().ToString() + ")", z);
                            s.ImageIndex = 14;
                            d.Add(s);
                            continue;
                        }
                        if (first.Value.GetType() == typeof(Hashtable))
                        {
                            List<TreeNode> h = new List<TreeNode>();
                            foreach (TreeNode w in getNodes((Hashtable)first.Value))
                            {
                                h.Add(w);
                            }
                            TreeNode[] z = h.ToArray();
                            s = new TreeNode(first.Key.ToString() + " (" + ((Hashtable)first.Value).Keys.Count.ToString() + ")", z);
                            s.ImageIndex = 15;
                            d.Add(s);
                            continue;
                        }
                    }
                    s = new TreeNode(first.Key.ToString() + " : obj? " + first.Value.ToString());
                    s.ImageIndex = 2;
                    d.Add(s);
                }
                else
                {
                    throw new Exception("The key must always be a string to convert it to a TreeNodeCollection");
                }
            }
            return d;
        }
    }
}
