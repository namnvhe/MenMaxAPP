using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenMaxBackEnd.Services
{
    public class ModelMapper
    {
        private readonly IMapper _mapper;

        public ModelMapper(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public TDestination Map<TDestination>(object source)
        {
            if (source == null)
                return default(TDestination);

            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
                return default(TDestination);

            return _mapper.Map<TSource, TDestination>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null || destination == null)
                return;

            _mapper.Map(source, destination);
        }

        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            if (source == null)
                return new List<TDestination>();

            return _mapper.Map<List<TDestination>>(source);
        }

        // Thêm method để map specific collections
        public List<TDestination> MapList<TDestination>(IEnumerable<object> source)
        {
            if (source == null)
                return new List<TDestination>();

            return source.Select(item => _mapper.Map<TDestination>(item)).ToList();
        }
    }
}
