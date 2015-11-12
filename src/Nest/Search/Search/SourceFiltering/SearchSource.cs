﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using static Nest.Static;

namespace Nest
{
	public interface ISourceFilter
	{
		[JsonProperty("include")]
		Fields Include { get; set; }

		[JsonProperty("exclude")]
		Fields Exclude { get; set; }
	}

	public class SourceFilter : ISourceFilter
	{
		public static SourceFilter ExcludeAll { get; } = new SourceFilter { Exclude = new [] {"*"} };
		public static SourceFilter IncludeAll { get; } = new SourceFilter { Include = new [] {"*"} };

		public Fields Include { get; set; }
		public Fields Exclude { get; set; }
	}

	public class SearchSourceDescriptor<T> : DescriptorBase<SearchSourceDescriptor<T>, ISourceFilter>, ISourceFilter
		where T : class
	{
		Fields ISourceFilter.Include { get; set; }

		Fields ISourceFilter.Exclude { get; set; }

		public SearchSourceDescriptor<T> Include(Func<FieldsDescriptor<T>, IPromise<Fields>> fields) => 
			Assign(a => a.Include = fields?.Invoke(new FieldsDescriptor<T>())?.Value);

		public SearchSourceDescriptor<T> Exclude(Func<FieldsDescriptor<T>, IPromise<Fields>> fields) => 
			Assign(a => a.Exclude = fields?.Invoke(new FieldsDescriptor<T>())?.Value);

	}
}