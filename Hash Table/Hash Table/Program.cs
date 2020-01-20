using System;

namespace Hash_Table
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating new instance of a hash table, some operations are demnstrated below
            var hashT = new HashTable();
            string[] hArr = hashT.HashArray;
            hashT.addHash("Aaryan");
            hashT.addHash("Aaryan P");
            hashT.addHash("Purohit");
            hashT.addHash("grlkglg$£%£%£$%hjklhkjtgnbdtjgh");
            hashT.deleteHash("Aaryan");
            Console.WriteLine(hashT.findItem("Purohit")); 
        }
    }
    class HashTable
    {
        //attributes include the array to store items with a hash index, as well as an array of linked lists in case of a collision
        private string[] hashArray;
        LinkedList[] listArray;
        public HashTable()
        {
            // setting size to 31, fairly large prime number
            hashArray = new string[31];
            listArray = new LinkedList[31];
        }
        public int hashItem(string item)
        {
            /*
             * HASHING ALGORITHM *
             * sum ASCII values of each character in given string
             * divide it by the sum of the ASCII values of the first and last character
             * mod this total by 31 (size of hash table)
             */
            int ascTot = 0;
            int k = item.Length;
            int hashIndex;
            foreach (char c in item)
            {
                ascTot += (int)c;
            }
            ascTot = ascTot / ((int)item[0] + (int)item[k - 1]);
            hashIndex = ascTot % 31;
            return hashIndex;
        }
        public void addHash(string item)
        {
            int hashIndex = hashItem(item);
            if (hashArray[hashIndex] == null)
            //if the space in the array index is empty, store the given item in it
            {
                hashArray[hashIndex] = item;
            }
            else
            {
                //if there is a collision, add item to linked list on that index
                if (listArray[hashIndex] == null)
                //if this is first instance of a collision, create a new linked list to allocate space for it
                {
                    listArray[hashIndex] = new LinkedList();
                    listArray[hashIndex].append(item);
                }
                else
                {
                    listArray[hashIndex].append(item);
                }

            }
        }
        public string findItem(string itemToFind)
        {
            int itemHash = hashItem(itemToFind);
            //foundItem is item currently in hash of itemToFind
            string foundItem = hashArray[itemHash];
            //generate hash index for item to be searched for
            if (foundItem == null)
            {
                Console.WriteLine("Item is not in hash table");
                return null;
            }
            //in case of collision (items with same hash), linked list must be searched
            else if (foundItem != itemToFind)
            {
                //checks to see if linked list at given index contains item
                Node finder = listArray[itemHash].searchItem(itemToFind);
                if (finder == null)
                {
                    Console.WriteLine("Item is not in hash table");
                    return null;
                }
                else
                {
                    //if found in linked list, node's data is returned
                    return finder.Data;
                }
            }
            else
            {
                return foundItem;
            }
        }
        public void deleteHash(string item)
        {
            int hashIndex = hashItem(item);
            //first checks to see if there is an item to delete
            if (findItem(item) == null)
            {
                Console.WriteLine("No item in table");
            }
            else if (hashArray[hashIndex] == item)
            {
                //if item is in hash array, it is deleted
                hashArray[hashIndex] = null;
                if (listArray[hashIndex] != null)
                {
                    //if there is a linked list connected to item to be deleted from hash array, head of collision linked list is popped and added to the actual hash table
                    string newHashItem = listArray[hashIndex].pop();
                    hashArray[hashIndex] = newHashItem;
                }
            }
            else 
            {
                //checks linked list connected to delete given item
                listArray[hashIndex].deleteNode(item);
            }
        }
        public double getLoadFactor()
        {
            return (hashArray.Length / 31);
        }
        public string[] HashArray
        {
            //getter and setter for the hash table array for testing
            get { return hashArray; }
            set { hashArray = value; }
        }
    }
    class Node
    {
        private Node next;
        private string data;
        public Node(string dataInput)
        {
            data = dataInput;
            next = null;
        }
        public Node Next
        {
            get { return next; }
            set { next = value; }
        }
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
    }
    class LinkedList
    {
        private Node head;
        public LinkedList()
        {
            head = null;
        }
        public Node Head
        {
            get { return head; }
        }
        public bool isEmpty()
        {
            if (head == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isFull()
        {
            return false;
        }
        public void append(string item)
        {
            if (isEmpty() == true)
            {
                head = new Node(item);
            }
            else
            {
                Node p = head;
                while (p.Next != null)
                {
                    p = p.Next;
                }
                p.Next = new Node(item);
            }
        }
        public int length()
        {
            Node p = head;
            if (p == null)
            {
                return 0;
            }
            else
            {
                int count = 1;
                while (p.Next != null)
                {
                    p = p.Next;
                    count++;
                }
                return count;
            }
        }
        public string pop()
        {
            if (isEmpty() == true)
            {
                Console.WriteLine("Error - List is empty");
                return ("0");
            }
            else
            {
                string popItem = head.Data;
                head = head.Next;
                return popItem;
            }
        }
        public void push(string item)
        {
            Node p = head;
            head = new Node(item);
            head.Next = p;
        }
        public int count(string userItem)
        {
            int itemCount = 0;
            Node p = head;
            while (p.Next != null)
            {
                if (p.Data == userItem)
                {
                    itemCount++;
                }
            }
            return itemCount;
        }
        public void deleteNode(string item)
        {
            Node p = head;
            if (head.Data == item)
            {
                head = p.Next;
            }
            else
            {
                do
                {
                    if (p.Next.Data == item)
                    {
                        break;
                    }
                    p = p.Next;
                } while (p.Next != null);
                p.Next = p.Next.Next;
            }
        }
        public Node searchItem(string itemToSearch)
        {
            Node p = head;
            bool found = false;
            if (isEmpty() == true)
            {
                Console.WriteLine("Linked List is empty");
                return null;
            }
            else
            {
                do
                {
                    if (p.Data == itemToSearch)
                    {
                        found = true;
                        break;
                    }
                    p = p.Next;
                } while (p.Next != null);
                if (found == true)
                {
                    return p;
                }
                else
                {
                    Console.WriteLine("Item not found");
                    Console.WriteLine(found);
                    return null;
                }
            }
        }
    }
}
