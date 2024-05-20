﻿using AssetRipper.GUI.Web.Pages;
using AssetRipper.GUI.Web.Paths;
using AssetRipper.Web.Content;

namespace AssetRipper.GUI.Web
{
	public abstract class DefaultPage : HtmlPage
	{
		public sealed override void Write(TextWriter writer)
		{
			base.Write(writer);
			using (new Html(writer).WithLang(Localization.CurrentLanguageCode).End())
			{
				using (new Head(writer).End())
				{
					new Meta(writer).WithCharset("utf-8").Close();
					new Meta(writer).WithName("viewport").WithContent("width=device-width, initial-scale=1.0").Close();
					new Title(writer).Close(GetTitle());
					Bootstrap.WriteStyleSheetReference(writer);
					new Link(writer).WithRel("stylesheet").WithHref("/css/site.css").Close();
				}
				using (new Body(writer).WithCustomAttribute("data-bs-theme", "dark").End())
				{
					WriteHeader(writer);

					using (new Div(writer).WithClass("container").End())
					{
						using (new Main(writer).WithRole("main").WithId("app").WithClass("pb-3").End())
						{
							WriteInnerContent(writer);
						}
					}

					WriteFooter(writer);

					WriteScriptReferences(writer);
				}
			}
		}

		public abstract string? GetTitle();

		public abstract void WriteInnerContent(TextWriter writer);

		private static void WriteHeader(TextWriter writer)
		{
			using (new Header(writer).End())
			{
				using (new Div(writer).WithClass("btn-group").End())
				{
					WriteFileMenu(writer);
					WriteExportMenu(writer);
				}
			}
		}

		private static void WriteFileMenu(TextWriter writer)
		{
			using (new Div(writer).WithClass("btn-group dropdown").End())
			{
				WriteDropdownButton(writer, Localization.MenuFile);
				using (new Ul(writer).WithClass("dropdown-menu").End())
				{
					using (new Li(writer).End())
					{
						new A(writer).WithClass("dropdown-item").WithHref("/Settings/Edit").Close(Localization.Settings);
					}
					using (new Li(writer).End())
					{
						WritePostLink(writer, "/LoadFolder", Localization.MenuFileOpenFolder, "dropdown-item");
					}
				}
			}
		}

		private static void WriteExportMenu(TextWriter writer)
		{
			using (new Div(writer).WithClass("btn-group dropdown").End())
			{
				WriteDropdownButton(writer, Localization.MenuExport);
				using (new Ul(writer).WithClass("dropdown-menu").End())
				{
					if (GameFileLoader.IsLoaded)
					{
						using (new Li(writer).End())
						{
							WritePostLink(writer, "/ExportAllAssets", Localization.MenuExportAll, "dropdown-item");
						}
						string version = GameFileLoader.GameBundle.GetMaxUnityVersion().ToString();
						using (new Li(writer).End())
						{
							new A(writer).WithClass("dropdown-item").WithNewTabAttributes().WithHref($"unityhub://{version}").Close(version);
						}
					}
					else
					{
						using (new Li(writer).End())
						{
							new A(writer).WithClass("dropdown-item disabled").WithCustomAttribute("aria-diabled", "true").Close(Localization.MenuExportAll);
						}
					}
				}
			}
		}

		private static void WriteDropdownButton(TextWriter writer, string buttonText)
		{
			new Button(writer).WithClass("btn btn-dark dropdown-toggle mx-0")
				.WithType("button")
				.WithCustomAttribute("data-bs-toggle", "dropdown")
				.WithCustomAttribute("aria-expanded", "false")
				.Close(buttonText);
		}

		public static void WritePostLink(TextWriter writer, string url, string name, string? @class = null)
		{
			using (new Form(writer).WithAction(url).WithMethod("post").End())
			{
				new Input(writer).WithType("submit").WithClass(@class).WithValue(name.ToHtml()).Close();
			}
		}

		private static void WriteFooter(TextWriter writer)
		{
			using (new Footer(writer).WithClass("border-top footer text-muted").End())
			{
				using (new Div(writer).WithClass("container text-center").End())
				{
					writer.Write("&copy; 2024 - UltraExtractor - Original program: AssetRipper");
					new A(writer).WithHref("/Privacy").Close(Localization.Privacy);
					writer.Write(" - ");
					new A(writer).WithHref("/Licenses").Close(Localization.Licenses);
				}
			}
		}

		protected virtual void WriteScriptReferences(TextWriter writer)
		{
			writer.Write("""<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js" integrity="sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r" crossorigin="anonymous"></script>""");
			Bootstrap.WriteScriptReference(writer);
			new Script(writer).WithSrc("/js/site.js").Close();
		}
	}
}
