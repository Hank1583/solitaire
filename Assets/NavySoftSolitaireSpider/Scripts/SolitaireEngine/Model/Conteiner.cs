namespace SolitaireEngine.Model 
{
	using System.Collections.Generic;

	public class Conteiner
	{
		private int id;
		private List<Card> element;

		private Conteiner (){}
		public Conteiner (int _id, List<Card> _element)
		{
			id = _id;
			element = _element;
		}
		public int Id {get{return id;}}
		public List<Card> Element {get{ return element;}}
		public Conteiner Copy()
		{
           

            int copyId = Id;
			List<Card> copyElement = new List<Card> ();
            foreach (Card element in Element)
            {
                //UnityEngine.Debug.Log(""+id+" " + element.Id);
                copyElement.Add(element.Copy());
            }
			Conteiner conteiner = new Conteiner (copyId,copyElement);
			return conteiner;
		}
		public void SetElement(List<Card> list)
		{
			element = list;
		}

        public void AddElement(Card item)
        {
            element.Add(item);
        }
    }
}