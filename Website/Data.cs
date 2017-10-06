using System.Collections.Generic;
using System.IO;

namespace Website
{

	public class Connfigger
	{
		public static string GetConnectionString()
		{
			//return @"";
			return @"Data Source=(local)\SQLEXPRESS;Initial Catalog=enelportal;Integrated Security=true;";
		}
		public static string GetTrackingCode()
		{
			//return "<!-- Start of StatCounter Code for Default Guide--><script type=\"text/javascript\"> var sc_project=10391388; var sc_invisible=1; var sc_security=\"51c01d0b\"; var scJsHost = ((\"https:\" == document.location.protocol) ? \"https://secure.\" : \"http://www.\"); document.write(\"<sc\"+\"ript type='text/javascript' src='\" + scJsHost + \"statcounter.com/counter/counter.js'></\"+\"script>\"); </script><noscript><div class=\"statcounter\"><a title=\"site stats\" href=\"http://statcounter.com/free-web-stats/\" target=\"_blank\"><img class=\"statcounter\" src=\"http://c.statcounter.com/10391388/0/51c01d0b/1/\" alt=\"site stats\"></a></div></noscript><!-- End of StatCounter Code for Default Guide -->";
			return "";
		}
		public static string DetectUILang(System.Web.HttpRequest request, Metadata metadata)
		{
			string uilang="";
			if(uilang=="" && request.Cookies["uilang"]!=null) { //try detecting uilang from cookie
				uilang=request.Cookies["uilang"].Value.ToLower();
				if(!metadata.isLangUI(uilang)) uilang="";
			}
			if(uilang=="" && request.UserLanguages!=null) { //try detecting uilang from browser setting
				foreach(string acceptlang in request.UserLanguages) {
					foreach(Language lang in metadata.languages) {
						if(lang.isUI) {
							if(acceptlang.ToLower()==lang.code || acceptlang.ToLower().StartsWith(lang.code+"-") || acceptlang.ToLower().StartsWith(lang.code+";")) {
								uilang=lang.code;
								break;
							}
						}
						if(uilang!="") break;
					}
				}
			} if(uilang=="") uilang="en";
			return uilang;
		}
	}
	
	public class Language
	{
		public string code, name, nativeName;
		public bool isUI, isCatalog, isSign;
		public Language(string code, string name, string nativeName, bool isUI) {
			this.code=code;
			this.name=name;
			this.nativeName=nativeName;
			this.isUI=isUI;
		}
		public Language(string code, string name, string nativeName, bool isUI, bool isSign)
		{
			this.code=code;
			this.name=name;
			this.nativeName=nativeName;
			this.isUI=isUI;
			this.isSign=isSign;
		}
	}

	public class DicType
	{
		public string code, name, legend;
		public DicType(string code, string name, string legend) {
			this.code=code;
			this.name=name;
			this.legend=legend;
		}
	}

	public class AboutPage
	{
		public string code="";
		public string title="";
		public AboutPage(string code, string title) {
			this.code=code;
			this.title=title;
		}
	}

	public class Metadata
	{
		public Metadata(string uilang, System.Web.HttpServerUtility server) {

			if(uilang!="") {
				string path=server.MapPath("/loc/"+uilang+".strings.txt");
				if(File.Exists(path)) {
					StreamReader reader = new StreamReader(path);
					while(reader.Peek()>-1) {
						string[] line=reader.ReadLine().Split('\t');
						if(line.Length>1) {
							this.strings.Add(line[0].Replace("$", ""), line[1]);
						}
					}
					reader.Close();
				}
			}

			this.languages=new List<Language>();
			this.languages.Add(new Language("cs", "Czech", "česky", true));
			this.languages.Add(new Language("cy", "Welsh", "Cymraeg", true));
			this.languages.Add(new Language("de", "German", "Deutsch", true));
			this.languages.Add(new Language("el", "Greek", "ελληνικά", true));
			this.languages.Add(new Language("en", "English", "English", true));
			this.languages.Add(new Language("es", "Spanish", "español", true));
			this.languages.Add(new Language("fr", "French", "français", true));
			this.languages.Add(new Language("fy", "Frisian", "Frysk", true));
			this.languages.Add(new Language("ga", "Irish", "Gaeilge", true));
			this.languages.Add(new Language("gd", "Scottish Gaelic", "Gàidhlig", true));
			this.languages.Add(new Language("hr", "Croatian", "hrvatski", true));
			this.languages.Add(new Language("hu", "Hungarian", "magyar", true));
			this.languages.Add(new Language("nl", "Dutch", "Nederlands", true));
			this.languages.Add(new Language("pl", "Polish", "polski", true));
			this.languages.Add(new Language("sco", "Scots", "Scots", true));
			this.languages.Add(new Language("fi", "Finnish", "suomi", true));

			this.languages.Add(new Language("sq", "Albanian", "", false));
			this.languages.Add(new Language("hy", "Armenian", "", false));
			this.languages.Add(new Language("rup", "Aromanian", "", false));
			this.languages.Add(new Language("ast", "Asturian", "", false));
			this.languages.Add(new Language("eu", "Basque", "", false));
			this.languages.Add(new Language("bg", "Bulgarian", "", false));
			this.languages.Add(new Language("ca", "Catalan", "", false));
			this.languages.Add(new Language("kw", "Cornish", "", false));
			this.languages.Add(new Language("da", "Danish", "", false));
			this.languages.Add(new Language("et", "Estonian", "", false));
			this.languages.Add(new Language("fo", "Faroese", "", false));
			this.languages.Add(new Language("gl", "Galician", "", false));
			this.languages.Add(new Language("he", "Hebrew", "", false));
			this.languages.Add(new Language("is", "Icelandic", "", false));
			this.languages.Add(new Language("it", "Italian", "", false));
			this.languages.Add(new Language("krl", "Karelian", "", false));
			this.languages.Add(new Language("kom", "Komi", "", false));
			this.languages.Add(new Language("la", "Latin", "", false));
			this.languages.Add(new Language("lv", "Latvian", "", false));
			this.languages.Add(new Language("lt", "Lithuanian", "", false));
			this.languages.Add(new Language("lb", "Luxembourgish", "", false));
			this.languages.Add(new Language("mk", "Macedonian", "", false));
			this.languages.Add(new Language("mt", "Maltese", "", false));
			this.languages.Add(new Language("gv", "Manx", "", false));
			this.languages.Add(new Language("mord", "Mordvinian", "", false));
			this.languages.Add(new Language("no", "Norwegian", "", false));
			this.languages.Add(new Language("pt", "Portuguese", "", false));
			this.languages.Add(new Language("ro", "Romanian", "", false));
			this.languages.Add(new Language("rm", "Romansh", "", false));
			this.languages.Add(new Language("ru", "Russian", "", false));
			this.languages.Add(new Language("smi", "Sami", "", false));
			this.languages.Add(new Language("sc", "Sardinian", "", false));
			this.languages.Add(new Language("sr", "Serbian", "", false));
			this.languages.Add(new Language("sk", "Slovak", "", false));
			this.languages.Add(new Language("sl", "Slovene", "", false));
			this.languages.Add(new Language("wen", "Sorbian", "", false));
			this.languages.Add(new Language("sv", "Swedish", "", false));
			this.languages.Add(new Language("tr", "Turkish", "", false));
			this.languages.Add(new Language("uk", "Ukrainian", "", false));
			this.languages.Add(new Language("vep", "Vepsian", "", false));
			this.languages.Add(new Language("yi", "Yiddish", "", false));
			this.languages.Add(new Language("br", "Breton", "", false));

			this.languages.Add(new Language("lmo", "Lombard", "", false));
			this.languages.Add(new Language("oc", "Occitan", "", false));

			this.languages.Add(new Language("sqk", "Albanian Sign Language", "", false, true));
			this.languages.Add(new Language("aen", "Armenian Sign Language", "", false, true));
			this.languages.Add(new Language("asq", "Austrian Sign Language", "", false, true));
			this.languages.Add(new Language("bfi", "British Sign Language", "", false, true));
			this.languages.Add(new Language("bqn", "Bulgarian Sign Language", "", false, true));
			this.languages.Add(new Language("csc", "Catalan Sign Language", "", false, true));
			this.languages.Add(new Language("csq", "Croatia Sign Language", "", false, true));
			this.languages.Add(new Language("cse", "Czech Sign Language", "", false, true));
			this.languages.Add(new Language("dsl", "Danish Sign Language", "", false, true));
			this.languages.Add(new Language("eso", "Estonian Sign Language", "", false, true));
			this.languages.Add(new Language("fss", "Finland-Swedish Sign Language", "", false, true));
			this.languages.Add(new Language("fse", "Finnish Sign Language", "", false, true));
			this.languages.Add(new Language("vgt", "Flemish Sign Language", "", false, true));
			this.languages.Add(new Language("sfb", "French Belgian Sign Language", "", false, true));
			this.languages.Add(new Language("fsl", "French Sign Language", "", false, true));
			this.languages.Add(new Language("gsg", "German Sign Language", "", false, true));
			this.languages.Add(new Language("gss", "Greek Sign Language", "", false, true));
			this.languages.Add(new Language("hsh", "Hungarian Sign Language", "", false, true));
			this.languages.Add(new Language("icl", "Icelandic Sign Language", "", false, true));
			this.languages.Add(new Language("isg", "Irish Sign Language", "", false, true));
			this.languages.Add(new Language("isr", "Israeli Sign Language", "", false, true));
			this.languages.Add(new Language("ise", "Italian Sign Language", "", false, true));
			this.languages.Add(new Language("lsl", "Latvian Sign Language", "", false, true));
			this.languages.Add(new Language("lls", "Lithuanian Sign Language", "", false, true));
			this.languages.Add(new Language("mdl", "Maltese Sign Language", "", false, true));
			this.languages.Add(new Language("vsi", "Moldova Sign Language", "", false, true));
			this.languages.Add(new Language("nsl", "Norwegian Sign Language", "", false, true));
			this.languages.Add(new Language("pso", "Polish Sign Language", "", false, true));
			this.languages.Add(new Language("psr", "Portuguese Sign Language", "", false, true));
			this.languages.Add(new Language("rms", "Romanian Sign Language", "", false, true));
			this.languages.Add(new Language("rsl", "Russian Sign Language", "", false, true));
			this.languages.Add(new Language("dse", "Sign Language of the Netherlands", "", false, true));
			this.languages.Add(new Language("svk", "Slovak Sign Language", "", false, true));
			this.languages.Add(new Language("ssp", "Spanish Sign Language", "", false, true));
			this.languages.Add(new Language("swl", "Swedish Sign Language", "", false, true));
			this.languages.Add(new Language("ssr", "Swiss-French Sign Language", "", false, true));
			this.languages.Add(new Language("sgg", "Swiss-German Sign Language", "", false, true));
			this.languages.Add(new Language("slf", "Swiss-Italian Sign Language", "", false, true));
			this.languages.Add(new Language("tsm", "Turkish Sign Language", "", false, true));
			this.languages.Add(new Language("ukl", "Ukrainian Sign Language", "", false, true));
			this.languages.Add(new Language("vsv", "Valencian Sign Language", "", false, true));
			this.languages.Add(new Language("ysl", "Yugoslavian Sign Language", "", false, true));

			
			if(uilang!="") {
				//Localize language names into the UI language:
				foreach(Language l in this.languages) {
					string locname=this.getString("lang-"+l.code);
					if(!locname.StartsWith("$")) l.name=this.getString("lang-"+l.code); //use default name (= English name) if no localized name is available
				}
			}

			//StreamWriter writer = new StreamWriter(server.MapPath("/loc/" + uilang + ".langs.txt"));
			//foreach(Language l in this.languages) {
			//	writer.WriteLine(l.code + "\t" + l.name);
			//};
			//writer.Close();

			this.dicTypes=new List<DicType>();
			this.dicTypes.Add(new DicType("gen", "General dictionaries", "[General dictionaries] are dictionaries that document contemporary vocabulary and are intended for everyday reference by native and fluent speakers."));
			this.dicTypes.Add(new DicType("por", "Portals and aggregators", "[Portals and aggregators] are websites that provide access to more than one dictionary and allow you to search them all at once."));
			this.dicTypes.Add(new DicType("lrn", "Learner's dictionaries", "[Learner's dictionaries] are intended for people who are learning the language as a second language."));
			this.dicTypes.Add(new DicType("spe", "Dictionaries on special topics", "[Dictionaries on special topics] are dictionaries that focus on a specific subset of the vocabulary (such as new words or phrasal verbs) or which focus on a specific dialect or variant of the language."));
			this.dicTypes.Add(new DicType("ort", "Spelling dictionaries", "[Spelling dictionaries] are dictionaries which codify the correct spelling and other aspects of the orthography of words."));
			this.dicTypes.Add(new DicType("ety", "Etymological dictionaries", "[Etymological dictionaries] are dictionaries that explain the origins of words."));
			this.dicTypes.Add(new DicType("his", "Historical dictionaries", "[Historical dictionaries] are dictionaries that document previous historical states of the language, or dictionaries that trace how the meanings and usage of words have evolved throughout history."));
			this.dicTypes.Add(new DicType("trm", "Terminological dictionaries", "[Terminological dictionaries] describe the vocabulary of specialized domains such as biology, mathematics or economics."));

			this.aboutPages.Add(new AboutPage("info", "About"));
			this.aboutPages.Add(new AboutPage("prop", "Suggest a dictionary"));
			this.aboutPages.Add(new AboutPage("crit", "Criteria for inclusion"));
			this.aboutPages.Add(new AboutPage("stmp", "Stamp of approval"));
			this.aboutPages.Add(new AboutPage("crtr", "Curator's manual"));
			this.aboutPages.Add(new AboutPage("dnld", "Download"));

			if(uilang!="") {
				foreach(DicType dt in this.dicTypes) {
					string locName=this.getString(dt.code+"-name");
					string locLegend=this.getString(dt.code+"-legend");
					if(!locName.StartsWith("$")) dt.name=locName;
					if(!locLegend.StartsWith("$"))dt.legend=locLegend;
				}

				foreach(AboutPage ap in this.aboutPages) {
					ap.title=this.getString("about-"+ap.code);
				}
			}
		}

		public List<Language> languages;
		public Language getLanguage(string code) {
			Language ret=new Language(code, code, code, false);
			foreach(Language l in this.languages) if(l.code==code) { ret=l; break; }
			return ret;
		}
		public string getNativeName(string code) {
			Language l=this.getLanguage(code);
			return l.nativeName;
		}
		public bool isLangUI(string langCode) {
			var ret=false;
			foreach(Language l in this.languages) if(l.code==langCode) { ret=true; break; }
			return ret;
		}

		public List<DicType> dicTypes;
		public DicType getDicType(string code)
		{
			DicType ret=new DicType(code, code, code);
			foreach(DicType l in this.dicTypes) if(l.code==code) { ret=l; break; }
			return ret;
		}

		public Dictionary<string, string> strings=new Dictionary<string, string>();
		public string getString(string code) {
			string ret="$"+code;
			if(this.strings.ContainsKey(code)) ret=this.strings[code]; else {
				if(code=="monoDicts") ret="Monolingual dictionaries";
			}
			return ret;
		}

		public List<AboutPage> aboutPages=new List<AboutPage>();
	}
}
