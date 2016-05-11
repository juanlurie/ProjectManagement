using Hermes.Ioc;
using MediaManagement.ApplicationService;
using MediaManagement.ApplicationService.Handlers;
using MediaManagement.Persistence.Queries;

namespace MediaManagement.Api
{
    public class DependencyRegistrar : IRegisterDependencies
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ImportFiles>();
            containerBuilder.RegisterType<SpotListQuery>();
            containerBuilder.RegisterType<MediaPlanItemCommentQuery>();
            containerBuilder.RegisterType<MediaPlanQuery>();
            containerBuilder.RegisterType<ShortCodeQuery>();
            containerBuilder.RegisterType<LeadQuery>();
            containerBuilder.RegisterType<TvProgrammeQuery>();
            containerBuilder.RegisterType<TvProgrammeService>();
            containerBuilder.RegisterType<ChangeLogQuery>();
            containerBuilder.RegisterType<MediaPlanItemExceptionQuery>();
            containerBuilder.RegisterType<MediaPlanService>();
            containerBuilder.RegisterType<MediaPlanItemCalculationService>();
            containerBuilder.RegisterType<MediaPlanItemExceptionStatusHistoryQuery>();
            containerBuilder.RegisterType<MediaPlanItemChangeStatusHistoryQuery>();
            containerBuilder.RegisterType<MediaPlanItemMatcher>();
            containerBuilder.RegisterType<EntityBaseProvider>();
            containerBuilder.RegisterType<RuleProcessor>();
        }
    }
}