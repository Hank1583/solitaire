namespace SolitaireEngine.Utility
{
	using System;
	using System.Collections.Generic;

	public class Utils
	{
		public Random random;
		public Utils()
		{
			random = new Random ();
		}
		public T RandomElement<T> (List<T> _lst)
		{
			int length = _lst.Count;
			int index = random.Next (0, length);
			return (T)_lst [index];
		}
		public List<T> ShuffleList<T> (List<T> _deck)
		{
			_deck.Sort ((x, y) => random.Next (0, 100) < 50 ? -1 : 1);
			return _deck;
		}
		public List<T> AddTo<T> (List<T> from, List<T> _to) // test it
		{
			from.ForEach (element => _to.Add (element));
			return _to;
		}
		private int uniqueID = 0;
		public int GenerateUniqueID ()
		{
			uniqueID++;
			return uniqueID;
		}
	}
}