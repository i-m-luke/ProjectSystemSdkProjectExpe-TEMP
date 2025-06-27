Specifikace požadavků:
- Potřebuji zaregistrovat custom project typu pro VisualStudio 2022
- Řešení musí být kompatibilní s moderním project systémem (CPS) - extension bude pracovat s SDK-style projekty
- Z online zdrojů není jasné, jak se má takové řešení vytvořit, proto musíš najít ty nejrelevantnější zdroje
- Přiložil jsem dokumentaci z oficiálního repozitáře pro project system

1. Detailní popis řešeného problému
Původní cíl: Vaším cílem je vytvořit rozšíření pro Visual Studio, které přidává vlastní šablonu projektu. Projekty vytvořené z této musí používat vlastní příponu souboru '.systestpack', ale jinak budou chovat naprosto identicky jako standardní C# projekty (.csproj). To znamená, že mají využívat C# projektový systém (CPS), umožňovat build, debug, přidávání souborů a další standardní operace.

Hlavní problém: Když je vytvořen projekt s příponou .systestpack, Visual Studio jej nerozpozná jako C# projekt. Místo toho, aby se načetl standardní C# projektový systém, Visual Studio se pokusilo projekt otevřít pomocí vaší vlastní, nově vytvořené "továrny na projekty" (ProjectFactory). Protože tato továrna byla zpočátku prázdná a neobsahovala žádnou logiku, výsledkem bylo, že se projekt buď neotevřel vůbec, nebo se otevřel jako "různé soubory" bez jakékoli projektové funkcionality.

Příčina problému: Problém spočíval v tom, že Visual Studio nebylo informováno, že vaše přípona .systest je pouze "příchuť" (flavor) standardního C# projektu. Bylo nutné explicitně zaregistrovat vaši ProjectFactory tak, aby fungovala jako obálka nad existujícím C# projektovým systémem. Bez této registrace se Visual Studio domnívalo, že .systest je zcela nový, nezávislý typ projektu, pro který nemá žádnou podporu.

2. Výpis vyzkoušených řešení
Během naší spolupráce jsme vyzkoušeli následující kroky a řešení:

Analýza a sjednocení souborů:

Zkontrolovali a sjednotili jsme obsah souborů .props a .targets, aby správně fungovaly s novou příponou .systest.
Upravili jsme soubor šablony .vstemplate, aby se při vytváření nového projektu korektně používal soubor s příponou .systest.
Implementace "Project Flavoring":

Identifikovali jsme, že správným řešením je technika zvaná "Project Flavoring" (agregace projektů).
Upravili jsme vaši třídu ProjectFactory.cs, aby dědila z Microsoft.VisualStudio.Shell.Flavor.FlavoredProjectFactoryBase.
V ProjectFactory.cs jsme implementovali metodu PreCreateForOuter, která je zodpovědná za vytvoření naší vlastní "flavor" třídy.
Vytvořili jsme novou třídu SystemTestFlavorProject.cs, která představuje naši vlastní logiku, jež se "navěsí" na základní C# projekt.
Registrace v balíčku rozšíření (VssdkPackage.cs):

Toto byl nejkritičtější a nejsložitější krok. Zkoušeli jsme různé způsoby, jak zaregistrovat naši továrnu a flavor.
Přidali jsme atribut [ProvideProjectFactory(...)] nad třídu VssdkPackage. Tento atribut říká Visual Studiu, že naše rozšíření poskytuje továrnu pro projekty s příponou .systest a že tato továrna je navázána na základní C# projektový systém (pomocí jeho GUID).
Důležité zjištění: Strávili jsme značný čas pokusy o použití atributu [ProvideProjectSubType]. Po několika neúspěšných pokusech o sestavení jsme zjistili, že tento atribut v sadě Visual Studio SDK neexistuje a jeho použití bylo příčinou chyb při kompilaci. Správná cesta je použít pouze [ProvideProjectFactory].
Správa závislostí a sestavení:

Do souboru SystemTestToolkit.Extension.csproj jsme přidali potřebné NuGet balíčky pro VS SDK a project flavoring.
Opakovaně jsme spouštěli sestavení (build) a čištění (clean) projektu pomocí msbuild z příkazové řádky, abychom odhalili a opravili chyby v konfiguraci a kódu.
Výsledkem těchto kroků je funkční řešení, kde je vaše ProjectFactory správně zaregistrována jako "flavor" pro C# projekty, což umožňuje Visual Studiu otevírat soubory .systest pomocí standardního C# projektového systému.