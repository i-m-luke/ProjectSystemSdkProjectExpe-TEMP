// Settings rules programatically see:
// https://github.com/dotnet/project-system/blob/5bc38a453f6342a115e225c3fe64316d1b00e31a/docs/repo/property-pages/how-to-add-a-new-project-property-page.md#option-3-iruleobjectprovider

//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Microsoft.Build.Framework.XamlTypes;
//using Microsoft.VisualStudio.ProjectSystem;
//using Microsoft.VisualStudio.ProjectSystem.Properties;

//namespace SystemTestToolkit.Extension.VSSDK.ProjectSystem;

//[ExportRuleObjectProvider(name: "RuleObjectProvider", context: "Project")]
//[AppliesTo(Constants.ProjectCapabilities.SystemTestPackage)]
//[Order(0)]
//internal class RuleObjectProvider : IRuleObjectProvider
//{
//    private struct RuleDefinition
//    {
//        public string Name { get; set; }

//        public string Description { get; set; }

//        public string DisplayName { get; set; }

//        public string PageTemplate { get; set; }

//        public int Order { get; set; }

//        public DataSource DataSource { get; set; }

//        public List<Category> Categories { get; set; }

//        public List<BaseProperty> Properties { get; set; }
//    };

//    private readonly Lazy<Rule[]> rules = new(CreateRules);

//    private static readonly RuleDefinition[] ruleDefinitions =
//    [
//        new RuleDefinition
//        {
//            Name = "ConfigurationGeneral",
//            DisplayName = "General",
//            Description = "General",
//            PageTemplate = "generic",
//            Order = 500,
//            Categories =
//            [
//                new()
//                {
//                    Name = "General",
//                    DisplayName = "General",
//                    Description = "General",
//                }
//            ],
//            DataSource = new DataSource
//            {
//                Persistence = "ProjectFile",
//                Label = "Configuration",
//                SourceOfDefaultValue = DefaultValueSourceLocation.AfterContext
//            },
//            Properties =
//            [
//                new StringProperty
//                {
//                    Name = "PackageID",
//                    DisplayName = "PackageID",
//                    Description = "ID of the package",
//                    Category = "General",
//                    ReadOnly = true,
//                    Visible = true,
//                    DataSource = new DataSource()
//                    {
//                        Persistence = "ProjectFile",
//                        Label = "Globals",
//                        HasConfigurationCondition = false
//                    }
//                }
//            ]
//        }
//    ];

//    public IReadOnlyCollection<Rule> GetRules() => this.rules.Value;

//    private static Rule[] CreateRules()
//    {
//        return ruleDefinitions.Select(definition =>
//        {
//            var rule = new Rule();

//            rule.BeginInit();

//            rule.Name = definition.Name;
//            rule.Description = definition.Description;
//            rule.DisplayName = definition.DisplayName;
//            rule.PageTemplate = definition.PageTemplate;
//            rule.Order = 500;

//            rule.Categories.AddRange(definition.Categories);
//            rule.DataSource = definition.DataSource;
//            rule.Properties.AddRange(definition.Properties);

//            rule.EndInit();

//            return rule;
//        }).ToArray();
//    }
//}

