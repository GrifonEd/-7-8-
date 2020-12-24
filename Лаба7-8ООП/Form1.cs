using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба7_8ООП
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
    public class ObjObserved
    {
        public Storage storage;
        public void AddStorage(Storage sto)
        {
            storage = sto;
        }
    }
    public abstract class Shape : ObjObserved
    {
        public PointF[] polygonPoints = new PointF[3];
        private string ClassName;
        public int R;
        public Color color;
        public int x, y;
        public bool choose = false;
        public bool narisovana = true;


        abstract public string Class_Name();
        abstract public string Move();
        abstract public string new_color();
        abstract public string Draw();
        abstract public string Save();
        abstract public string Load();
        abstract public string Show();


    }
    public class Observer
    {
        public virtual void SubjectChanged() { return; }
    }
    public class Observed
    {
        private List<Observer> observers;
        public Observed()
        {   
            observers = new List<Observer>();
        }
        public void AddObserver(Observer o)
        {
            observers.Add(o);
        }
        public void Notify()
        {
            foreach (Observer observer in observers) observer.SubjectChanged();
        }
    }
    public class Storage : Observed
    {

        public Shape[] objects;
        public Storage(int size)
        {
            objects = new Shape[size];
            for (int i = 0; i < size; i++)
                objects[i] = null;
        }
        public void add_object(ref int size, ref Shape new_object, int ind, ref int index_sozdania)
        {
            Storage storage1 = new Storage(size + 1);
            for (int i = 0; i < size; i++)
                storage1.objects[i] = objects[i];
            size = size + 1;
            videlenie(size);
            for (int i = 0; i < size; i++)
                objects[i] = storage1.objects[i];
            objects[ind] = new_object;
            index_sozdania = ind;
        }
        public Shape GetObject(int index)
        {
            return objects[index];
        }
        public void videlenie(int size)
        {
            objects = new Shape[size];
            for (int i = 0; i < size; i++)
                objects[i] = null;
        }
        public int kolvo_zanyatix(int size)
        {
            int count_zanyatih = 0;
            for (int i = 0; i < size; i++)
            {
                if (!proverka(i))
                    count_zanyatih++;
            }
            return count_zanyatih;
        }
        public bool proverka(int kolvo_elem)
        {
            if (objects[kolvo_elem] == null)
                return true;
            else return false;
        }
        public void Delte_obj(ref int kolvo_elem)
        {
            if (objects[kolvo_elem] != null)
            {
                objects[kolvo_elem] = null;
                kolvo_elem--;
            }
        }

    }
    class Tree : Observer
    {
        private Storage<Shape> sto;
        private TreeView tree;
        public Tree(Storage<Shape> sto, TreeView tree)
        {
            this.sto = sto;
            this.tree = tree;
        }

        public void Print()
        {
            tree.Nodes.Clear();
            if (sto.size() != 0)
            {
                int SelectedIndex = 0;
                TreeNode start = new TreeNode("Фигуры");
                sto.toFirst();
                for (int i = 0; i < sto.size(); i++, sto.next())
                {
                    if (sto.getCurPTR() == sto.getIteratorPTR()) SelectedIndex = i;
                    PrintNode(start, sto.getIterator());
                }
                tree.Nodes.Add(start);

                for (int i = 0; i < sto.size(); i++)
                {
                    sto.next();
                    tree.SelectedNode = tree.Nodes[0].Nodes[i];
                    if (sto.isChecked() == true)
                        tree.SelectedNode.ForeColor = Color.Red;
                    else tree.SelectedNode.ForeColor = Color.Black;
                }
            }
            tree.ExpandAll();

        }

        private void PrintNode(TreeNode node, Shape shape)
        {
            if (shape is SGroup)
            {
                TreeNode tn = new TreeNode(shape.Show());
                if (((SGroup)shape).sto.size() != 0)
                {
                    ((SGroup)shape).sto.toFirst();
                    for (int i = 0; i < ((SGroup)shape).sto.size(); i++, ((SGroup)shape).sto.next())
                        PrintNode(tn, ((SGroup)shape).sto.getIterator());
                }
                node.Nodes.Add(tn);
            }
            else
            {

                node.Nodes.Add(shape.GetInfo());
            }
        }

        public override void SubjectChanged()
        {
            Print();
        }
    }

}
