﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<ConstantScoreQueryDescriptor<object>>))]
	public interface IConstantScoreQuery : IQuery
	{
		[JsonProperty(PropertyName = "filter")]
		QueryContainer Filter { get; set; }
	}

	public class ConstantScoreQuery : QueryBase, IConstantScoreQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public string Lang { get; set; }
		public string Script { get; set; }
		public Dictionary<string, object> Params { get; set; }
		public QueryContainer Filter { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.ConstantScore = this;
		internal static bool IsConditionless(IConstantScoreQuery q) => q.Filter == null;
	}

	public class ConstantScoreQueryDescriptor<T> 
		: QueryDescriptorBase<ConstantScoreQueryDescriptor<T>, IConstantScoreQuery>
		, IConstantScoreQuery where T : class
	{
		bool IQuery.Conditionless => ConstantScoreQuery.IsConditionless(this);
		QueryContainer IConstantScoreQuery.Filter { get; set; }

		public ConstantScoreQueryDescriptor<T> Filter(Func<QueryContainerDescriptor<T>, QueryContainer> selector) => 
			Assign(a => a.Filter = selector?.Invoke(new QueryContainerDescriptor<T>()));
	}
}