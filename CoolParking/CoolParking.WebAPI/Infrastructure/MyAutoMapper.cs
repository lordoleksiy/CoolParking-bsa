using AutoMapper;

namespace CoolParking.WebAPI.Infrastructure
{
    public class MyAutoMapper<TSourse, TDestination>
    {
        private readonly Mapper mapper;
        public MyAutoMapper()
        {
            var cfg = new MapperConfiguration(c => c.CreateMap<TSourse, TDestination>());
            mapper = new Mapper(cfg);
        }

        public TDestination Map(TSourse value)
        {
            return mapper.Map<TDestination>(value);
        }

        public TEnumerableDestination Map<TEnumerableSource, TEnumerableDestination>(TEnumerableSource value)
        {
            return mapper.Map<TEnumerableSource, TEnumerableDestination>(value);
        }
    }
}
