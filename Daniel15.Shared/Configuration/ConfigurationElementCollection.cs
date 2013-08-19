using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Daniel15.Shared.Configuration
{
	/// <summary>
	/// Strongly-typed version of <see cref="ConfigurationElementCollection"/>.
	/// Repesents a collection of configuration elements.
	/// </summary>
	/// <typeparam name="TKey">Type of the key</typeparam>
	/// <typeparam name="TElement">Type of the configuration element</typeparam>
	public abstract class ConfigurationElementCollection<TKey, TElement>
		: ConfigurationElementCollection,
		IList<TElement>, IDictionary<TKey, TElement>
		where TElement : ConfigurationElement, new()
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new TElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return GetElementKey((TElement)element);
		}

		public abstract TKey GetElementKey(TElement element);

		#region Implementation of IEnumerable<out TElement>
		public new IEnumerator<TElement> GetEnumerator()
		{
			//return this.Cast<TElement>().GetEnumerator();
			var iterator = base.GetEnumerator();
			while (iterator.MoveNext())
			{
				yield return (TElement)iterator.Current;
			}
		}
		#endregion

		#region Implementation of ICollection<TElement>
		public void Add(TElement item)
		{
			BaseAdd(item);
		}

		public void Clear()
		{
			BaseClear();
		}

		public bool Contains(TElement item)
		{
			return BaseIndexOf(item) > -1;
		}

		public void CopyTo(TElement[] array, int arrayIndex)
		{
			Array.Copy(this.ToArray<TElement>(), 0, array, arrayIndex, Count);
		}

		public bool Remove(TElement item)
		{
			BaseRemove(GetElementKey(item));
			return true;
		}

		public new bool IsReadOnly
		{
			get { return IsReadOnly(); }
		}
		#endregion

		#region Implementation of IList<TElement>
		public int IndexOf(TElement item)
		{
			return BaseIndexOf(item);
		}

		public void Insert(int index, TElement item)
		{
			BaseAdd(index, item);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public TElement this[int index]
		{
			get { return (TElement)BaseGet(index); }
			set
			{
				BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}
		#endregion

		#region Implementation of IEnumerable<out KeyValuePair<TKey,TElement>>
		IEnumerator<KeyValuePair<TKey, TElement>> IEnumerable<KeyValuePair<TKey, TElement>>.GetEnumerator()
		{
			var iterator = base.GetEnumerator();
			while (iterator.MoveNext())
			{
				var item = (TElement)iterator.Current;
				yield return new KeyValuePair<TKey, TElement>(GetElementKey(item), item);
			}
		}
		#endregion

		#region Implementation of ICollection<KeyValuePair<TKey,TElement>>
		public void Add(KeyValuePair<TKey, TElement> item)
		{
			Add(item.Value);
		}

		public bool Contains(KeyValuePair<TKey, TElement> item)
		{
			return Contains(item.Value);
		}

		public void CopyTo(KeyValuePair<TKey, TElement>[] array, int arrayIndex)
		{
			Array.Copy(((IDictionary<TKey, TElement>)this).ToArray(), 0, array, arrayIndex, Count);
		}

		public bool Remove(KeyValuePair<TKey, TElement> item)
		{
			BaseRemove(item.Key);
			return true;
		}
		#endregion

		#region Implementation of IDictionary<TKey,TElement>
		public bool ContainsKey(TKey key)
		{
			return BaseGet(key) != null;
		}

		public void Add(TKey key, TElement value)
		{
			Add(value);
		}

		public bool Remove(TKey key)
		{
			BaseRemove(key);
			return true;
		}

		public bool TryGetValue(TKey key, out TElement value)
		{
			value = (TElement)BaseGet(key);
			return value != null;
		}

		public TElement this[TKey key]
		{
			get { return (TElement)BaseGet(key); }
			set
			{
				BaseRemove(key);
				BaseAdd(value);
			}
		}

		public ICollection<TKey> Keys
		{
			get { return BaseGetAllKeys().Cast<TKey>().ToList(); }
		}
		public ICollection<TElement> Values
		{
			get
			{
				var collection = new List<TElement>();
				foreach (var item in this)
				{
					collection.Add(item);
				}
				return collection;
			}
		}
		#endregion
	}
}
