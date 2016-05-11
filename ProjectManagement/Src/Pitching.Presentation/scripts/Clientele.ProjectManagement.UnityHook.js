var ProjectManagement = angular.module('Clientele.ProjectManagement', ['pasvaz.bindonce', 'ui.grid.infiniteScroll', 'ui.grid.resizeColumns', 'ui.grid.selection', 'ui.grid.exporter', 'ui.grid.pinning']);

(function () {

    var scripts = [];
    var applicationId = "Clientele.ProjectManagement";
    var application = applicationHost.retrieveApplicationById(applicationId);
    var configuration = applicationHost.retrieveApplicationConfigurationById(applicationId);
    var sourceUrl = configuration.UnityUrl;

    var filesToInclude = [];

    Enumerable.From(filesToInclude).ForEach(function (x) {
        scripts.push(sourceUrl + x.FileName);
    });

    var registerApplicationInUnity = function () {
        LazyLoad.js(scripts, function () {

            ProjectManagement.config(function ($routeProvider) {
                var componentKey = "ProjectManagement";
                var componentUrlPrefix = "/" + componentKey + "/";
            })
      .run(['unityApplicationRepository', function (unityApplicationRepository) {

          var myApplicationGuid = "813f966d-5d41-4aea-941a-a13246f410b4";
          var componentKey = "ProjectManagement";

          var projectManagementConfiguration = {
              Id: myApplicationGuid,
              applicationName: "Clientèle Project Management",
              IdentityPrefix: ""
          };

          var titleBarNavigation = [];
          
          unityApplicationRepository.addApplication(componentKey, titleBarNavigation, projectManagementConfiguration);
      }]);

            applicationHost.completeApplicationRegistration(applicationId);
        });
    }

    application.RegisterApplication = registerApplicationInUnity;

})();
