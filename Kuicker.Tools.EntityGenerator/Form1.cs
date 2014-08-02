using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kuicker.Data;

namespace Kuicker.Tools.EntityGenerator
{
	public partial class frmMain : Form
	{
		#region DllImport
		[DllImport("user32.dll", EntryPoint = "SetWindowText")]
		private static extern int SetWindowText(
			IntPtr hWnd, string text
		);

		[DllImport("user32.dll", EntryPoint = "FindWindowEx")]
		private static extern IntPtr FindWindowEx(
			IntPtr hwndParent,
			IntPtr hwndChildAfter,
			string lpszClass,
			string lpszWindow
		);

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(
			IntPtr hWnd,
			int uMsg,
			int wParam,
			string lParam
		);
		#endregion

		public const string _Name = "EntityGenerator";

		public IQueryable<TableSchema> TableSchemas { get; set; }
		public IQueryable<ViewSchema> ViewSchemas { get; set; }
		public IQueryable<PackageSchema> PackageSchemas { get; set; }
		public ISqlFormater Formater { get; set; }

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			try {
				foreach(var providerName in Constants.DbProvider.Names) {
					cbbProviders.Items.Add(providerName);
				}
				cbbProviders.SelectedItem = Config
					.Application.Get(_Name, "Provider").ToString();
				txtConnectionString.Text = Config
					.Application.Get(_Name, "ConnectionString").ToString();

				txtNamespace.Text = Config
					.Application.Get(_Name, "Namespace").ToString();

				txtTableTrimPrefix.Text = Config
					.Application.Get(_Name, "TableTrimPrefix").ToString();
				txtTableAppendSuffix.Text = Config
					.Application.Get(_Name, "TableAppendSuffix").ToString();

				txtColumnTrimPrefix.Text = Config
					.Application.Get(_Name, "ColumnTrimPrefix").ToString();
				txtColumnAppendSuffix.Text = Config
					.Application.Get(_Name, "ColumnAppendSuffix").ToString();

				txtPackageTrimPrefix.Text = Config
					.Application.Get(_Name, "PackageTrimPrefix").ToString();
				txtPackageAppendSuffix.Text = Config
					.Application.Get(_Name, "PackageAppendSuffix").ToString();

				txtProcedureTrimPrefix.Text = Config
					.Application.Get(_Name, "ProcedureTrimPrefix").ToString();

				txtArgumentTrimPrefix.Text = Config
					.Application.Get(_Name, "ArgumentTrimPrefix").ToString();

				txtSkipPrefix.Text = SplitPattern(
					Config
						.Application
						.Get(_Name, "SkipPrefix")
						.ToString()
				).Join(", ");

				txtKeyword.Text = SplitPattern(
					Config
						.Application
						.Get(_Name, "Keyword")
						.ToString()
				).Join(", ");

				txtPrefix.Text = SplitPattern(
					Config
						.Application
						.Get(_Name, "Prefix")
						.ToString()
				).Join(", ");

				txtSuffix.Text = SplitPattern(
					Config
						.Application
						.Get(_Name, "Suffix")
						.ToString()
				).Join(", ");

				txtAbbreviation.Text = SplitPattern(
					Config
						.Application
						.Get(_Name, "Abbreviation")
						.ToString()
				).Join(", ");

				txtOutputPath.Text = Config
					.Application.Get(_Name, "OutputPath").ToString();

			} catch(Exception ex) {
				MessageBox.Show(
					ex.Message,
					_Name,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void lbxTables_SelectedIndexChanged(
			object sender, EventArgs e)
		{
			ListBox lbx = sender as ListBox;
			if(null == lbx) { return; }

			clbColumns.Items.Clear();
			txtCode.Text = string.Empty;
			string name = lbx.SelectedItem.ToString();
			if(name.IsNullOrEmpty()) { return; }

			var obj = TableSchemas.FirstOrDefault(x =>
				x.TableName == name
			);
			if(null == obj) { return; }
			string columnPrefix = string.Empty;
			bool columnPrefixDone = false;
			foreach(var column in obj.Columns) {
				clbColumns.Items.Add(column.ColumnName, true);

				if(!columnPrefixDone) {
					string[] parts = column.ColumnName.Split('_');
					if(columnPrefix.IsNullOrEmpty()) {
						if(parts.Length > 1) {
							columnPrefix = parts[0];
						} else {
							columnPrefixDone = true;
						}
					} else {
						if(columnPrefix != parts[0]) {
							columnPrefix = string.Empty;
						} else {
							columnPrefix += "_";
						}
						columnPrefixDone = true;
					}
				}
			}

			RefreshTable(name);
		}


		private void btnReflesh_Click(object sender, EventArgs e)
		{
			if(null == cbbProviders.SelectedItem) {
				MessageBox.Show(
					"Select Data Provider first!",
					frmMain._Name,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);

				return;
			}

			this.BackColor = SystemColors.ControlDark;
			lbxTables.Items.Clear();
			clbColumns.Items.Clear();

			DataSettings.Add(
				frmMain._Name,
				txtConnectionString.Text,
				cbbProviders.SelectedItem.ToString()
			);

			DoneSchema = false;
			Task.Factory.StartNew(() => ReadSchema());
		}


		private bool DoneSchema = false;

		private void ReadSchema()
		{
			if(!DoneSchema) {
				using(var api = new EntityApi(_Name)) {
					Formater = api.Formater;
					TableSchemas = api.Builder.TableSchemas;
					ViewSchemas = api.Builder.ViewSchemas;
					PackageSchemas = api.Builder.PackageSchemas;
				}
				DoneSchema = true;
			}

			if(InvokeRequired) {
				Invoke(
					new MethodInvoker(ReadSchema)
				);

			} else {
				try {
					foreach(var one in TableSchemas) {
						if(Skip(one.TableName)) { continue; }
						lbxTables.Items.Add(one.TableName);
					}

					foreach(var one in ViewSchemas) {
						if(Skip(one.ViewName)) { continue; }
						lbxTables.Items.Add(one.ViewName);
					}

					foreach(var one in PackageSchemas) {
						if(Skip(one.PackageName)) { continue; }
						lbxPackages.Items.Add(one.PackageName);
					}

				} catch(Exception ex) {
					MessageBox.Show(
						ex.Message,
						frmMain._Name,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);

				} finally {
					this.BackColor = SystemColors.Control;
				}
			}
		}


		private Tuple<
			ISqlFormater,
			IQueryable<TableSchema>,
			IQueryable<ViewSchema>,
			IQueryable<PackageSchema>>
			RefreshTask(object provider, string connectionString)
		{

			if(null == provider) {
				MessageBox.Show(
					"Select Data Provider first!",
					frmMain._Name,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				new Tuple<
					ISqlFormater,
					IQueryable<TableSchema>,
					IQueryable<ViewSchema>,
					IQueryable<PackageSchema>>(null, null, null, null);
			}

			DataSettings.Add(
				frmMain._Name,
				connectionString,
				provider.ToString()
			);



			try {
				ISqlFormater formater;
				IQueryable<TableSchema> tableSchemas;
				IQueryable<ViewSchema> viewSchemas;
				IQueryable<PackageSchema> packageSchemas;

				using(var api = new EntityApi(_Name)) {
					formater = api.Formater;
					tableSchemas = api.Builder.TableSchemas;
					viewSchemas = api.Builder.ViewSchemas;
					packageSchemas = api.Builder.PackageSchemas;
				}
				return new Tuple<
					ISqlFormater,
					IQueryable<TableSchema>,
					IQueryable<ViewSchema>,
					IQueryable<PackageSchema>>(
						formater,
						tableSchemas,
						viewSchemas,
						packageSchemas
					);
			} catch(Exception ex) {
				MessageBox.Show(
					ex.Message,
					frmMain._Name,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return new Tuple<
					ISqlFormater,
					IQueryable<TableSchema>,
					IQueryable<ViewSchema>,
					IQueryable<PackageSchema>>(null, null, null, null);
			}

		}

		private void OpenToNotepad()
		{
			string title = "XxxEntity";
			string code = "public class XxxEntity ...";

			Process notepad = Process.Start(
				new ProcessStartInfo("notepad.exe")
			);
			notepad.WaitForInputIdle();

			if(!string.IsNullOrEmpty(title)) {
				SetWindowText(notepad.MainWindowHandle, title);
			}

			if(notepad != null && !string.IsNullOrEmpty(code)) {
				IntPtr child = FindWindowEx(
					notepad.MainWindowHandle,
					new IntPtr(0),
					"Edit",
					null
				);
				SendMessage(child, 0x000C, 0, code);
			}
		}


		private void RefreshTable(string name)
		{
			txtCode.Text = GenerateEntitySchema(name);
		}


		private void RefreshPackage(string name)
		{
			txtCode.Text = GeneratePackageSchema(name);
		}

		private void btnSelectPath_Click(object sender, EventArgs e)
		{
			if(fbDialog.ShowDialog() == DialogResult.OK) {
				this.txtOutputPath.Text = fbDialog.SelectedPath;
			}
		}


		private void btnOutput_Click(object sender, EventArgs e)
		{
			Task.Factory.StartNew(() => OutputSchema());
		}

		// {OutputPath}
		//   +-- Entity
		//         +-- Schema
		//         +-- Implement
		//   +-- Package
		//         +-- Schema
		//         +-- Implement
		private void OutputSchema() {
			//if(txtOutputPath.Text.IsNullOrEmpty()) {
			//	MessageBox.Show(
			//		"Choose Output Path first!",
			//		_Name,
			//		MessageBoxButtons.OK,
			//		MessageBoxIcon.Warning
			//	);
			//	return;
			//}

			var entityPath = Path.Combine(
				txtOutputPath.Text, "Entity"
			);
			var entitySchemaPath = Path.Combine(
				txtOutputPath.Text, "Entity", "Schema"
			);
			var entityImplementPath = Path.Combine(
				txtOutputPath.Text, "Entity", "Implement"
			);
			var packagePath = Path.Combine(
				txtOutputPath.Text, "Package"
			);
			var packageSchemaPath = Path.Combine(
				txtOutputPath.Text, "Package", "Schema"
			);
			var packageImplementPath = Path.Combine(
				txtOutputPath.Text, "Package", "Implement"
			);

			if(!Directory.Exists(entityPath)) {
				Directory.CreateDirectory(entityPath);
			}
			if(!Directory.Exists(entitySchemaPath)) {
				Directory.CreateDirectory(entitySchemaPath);
			}
			if(!Directory.Exists(entityImplementPath)) {
				Directory.CreateDirectory(entityImplementPath);
			}
			if(!Directory.Exists(packagePath)) {
				Directory.CreateDirectory(packagePath);
			}
			if(!Directory.Exists(packageSchemaPath)) {
				Directory.CreateDirectory(packageSchemaPath);
			}
			if(!Directory.Exists(packageImplementPath)) {
				Directory.CreateDirectory(packageImplementPath);
			}


			string code;
			string codePath;

			int entityCount = 0;
			foreach(var table in TableSchemas) {
				if(Skip(table.TableName)) { continue; }
				var tableName = table.TableName;
				var entityName = tableName
					.TrimStart(txtTableTrimPrefix.Text);
				entityName = NamingFilter(entityName)
					.AppendSuffix(txtTableAppendSuffix.Text);
				if(entityName.IsNullOrEmpty()) {
					entityName = tableName;
				}

				//
				code = GenerateEntitySchema(tableName);
				codePath = Path
					.Combine(
						entitySchemaPath,
						entityName.ToPopularCodeName() + ".cs"
					);
				if(File.Exists(codePath)) { File.Delete(codePath); }
				File.WriteAllText(codePath, code, Encoding.UTF8);

				//
				code = GenerateEntityImplement(tableName);
				codePath = Path
					.Combine(
						entityImplementPath,
						entityName.ToPopularCodeName() + ".cs"
					);
				if(File.Exists(codePath)) { File.Delete(codePath); }
				File.WriteAllText(codePath, code, Encoding.UTF8);

				entityCount++;
			}

			int packageCount = 0;
			foreach(var package in PackageSchemas) {
				if(Skip(package.PackageName)) { continue; }
				var packageName = package.PackageName;
				var packageClassName = packageName
					.TrimStart(txtPackageTrimPrefix.Text);
				packageClassName = NamingFilter(packageClassName)
					.AppendSuffix(txtPackageAppendSuffix.Text);
				if(packageClassName.IsNullOrEmpty()) {
					packageClassName = packageName;
				}

				//
				code = GeneratePackageSchema(packageName);
				codePath = Path
					.Combine(
						packageSchemaPath,
						packageClassName.ToPopularCodeName() + ".cs"
					);
				if(File.Exists(codePath)) { File.Delete(codePath); }
				File.WriteAllText(codePath, code, Encoding.UTF8);

				//
				code = GeneratePackageImplement(packageName);
				codePath = Path
					.Combine(
						packageImplementPath,
						packageClassName.ToPopularCodeName() + ".cs"
					);
				if(File.Exists(codePath)) { File.Delete(codePath); }
				File.WriteAllText(codePath, code, Encoding.UTF8);

				packageCount++;
			}

			var dResult = MessageBox.Show(
				string.Format(
					new[]{
						"Generate {0} tables and {1} packages, ",
						"whether to open the output directory?",
					}.Join(),
					entityCount,
					packageCount
				),
				_Name,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Information
			);
			//if(DialogResult.Yes == dResult) {
			//	Process.Start(txtOutputPath.Text);
			//}
		}


		private string GenerateEntityImplement(string name)
		{
			var pattern =
@"// {EntityName}.cs
//
// Generated by Kuicker.Tools.EntityGenerator.
// {Today}

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Kuicker;
using Kuicker.Data;

namespace {Namespace}
{
	/// <summary>
	/// {Comments}
	/// </summary>
	[Description(""{Comments}"")]
	[DataContract, Table]
	public partial class {EntityName} : Entity<{EntityName}>
	{
		#region constructor
		public {EntityName}()
			: base()
		{
		}
		#endregion

		#region IEntity
		#endregion

		#region static
		#endregion

		#region instance
		#endregion

		#region private
		#endregion

		#region event handler
		#endregion
	}
}";
			var now = DateTime.Now.yyyy_MM_dd_HH_mm_ss_fff();
			var nameSpace = txtNamespace.Text;
			var tableName = name;
			var entityName = tableName
				.TrimStart(txtTableTrimPrefix.Text);
			entityName = NamingFilter(entityName)
				.AppendSuffix(txtTableAppendSuffix.Text);
			if(entityName.IsNullOrEmpty()) {
				entityName = tableName;
			}

			var tableSchema = TableSchemas.FirstOrDefault(x =>
				x.TableName == tableName
			);

			var sb = new StringBuilder();
			sb.Append(pattern);
			sb
				.Replace(
					"{Comments}",
					tableSchema.Comments.AirBag(tableSchema.TableName)
				)
				.Replace("{Today}", now)
				.Replace("{Namespace}", nameSpace)
				.Replace("{EntityName}", entityName)
				.Replace("{TableName}", tableName);

			return sb.ToString();
		}


		private string GenerateEntitySchema(string name)
		{
			var pattern =
@"// {EntityName}.cs
//
// Generated by Kuicker.Tools.EntityGenerator.
// {Today}

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Kuicker;
using Kuicker.Data;

namespace {Namespace}
{
	public partial class {EntityName} : Entity<{EntityName}>
	{
		public override string __TableName
		{
			get
			{
				return ""{TableName}"";
			}
		}
{Properties}
	}
}";
			var now = DateTime.Now.yyyy_MM_dd_HH_mm_ss_fff();
			var nameSpace = txtNamespace.Text;
			var tableName = name;
			var entityName = tableName
				.TrimStart(txtTableTrimPrefix.Text);
			entityName = NamingFilter(entityName)
				.AppendSuffix(txtTableAppendSuffix.Text);
			if(entityName.IsNullOrEmpty()) {
				entityName = tableName;
			}

			var tableSchema = TableSchemas.FirstOrDefault(x =>
				x.TableName == tableName
			);

			var ef = EnumCache.Get<DataFormat>();
			var sbProperties = new StringBuilder();
			foreach(var column in tableSchema.Columns) {
				var propertyName = column.ColumnName
					.TrimStart(txtColumnTrimPrefix.Text);
				propertyName = NamingFilter(propertyName)
					.AppendSuffix(txtColumnAppendSuffix.Text);
				if(propertyName.IsNullOrEmpty()) {
					propertyName = column.ColumnName;
				}
				
				var columnSchema = tableSchema.Columns.FirstOrDefault(x =>
					x.ColumnName == column.ColumnName
				);
				if(columnSchema == null) { continue; }

				if(sbProperties.Length > 0) {
					sbProperties.AppendLine();
				}

				sbProperties.AppendFormat(@"
		/// <summary>
		/// {0}
		/// </summary>
		[Description(""{0}"")]
		[DataMember, Column(""{1}""){2}{3}]
		public {4} {5} {{ get; set; }}",
					column.Comments.AirBag(column.ColumnName),
					column.ColumnName,
					tableSchema
						.IsPrimaryKey(column.ColumnName)
						.If(", PrimaryKey"),
					column
						.Nullable
						.If(", AllowDBNull"),
					columnSchema.ToTypeName(Formater),
					propertyName
				);
			}

			var sb = new StringBuilder();
			sb.Append(pattern);
			sb
				.Replace(
					"{Comments}",
					tableSchema.Comments.AirBag(tableSchema.TableName)
				)
				.Replace("{Today}", now)
				.Replace("{Namespace}", nameSpace)
				.Replace("{EntityName}", entityName)
				.Replace("{TableName}", tableName)
				.Replace("{Properties}", sbProperties.ToString());

			return sb.ToString();
		}

		private string GeneratePackageImplement(string name)
		{
			var pattern =
@"// {PackageClassName}.cs
//
// Generated by Kuicker.Tools.EntityGenerator.
// {Today}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kuicker;
using Kuicker.Data;

namespace {Namespace}
{
	/// <summary>
	/// {Comments}
	/// </summary>
	[Description(""{Comments}"")]
	[Package]
	public partial class {PackageClassName} : IPackage
	{
	}
}";
			var now = DateTime.Now.yyyy_MM_dd_HH_mm_ss_fff();
			var nameSpace = txtNamespace.Text;
			var packageName = name;
			var packageClassName = packageName
				.TrimStart(txtPackageTrimPrefix.Text);
			packageClassName = NamingFilter(packageClassName)
				.AppendSuffix(txtPackageAppendSuffix.Text);
			if(packageClassName.IsNullOrEmpty()) {
				packageClassName = packageName;
			}

			var packageSchema = PackageSchemas.FirstOrDefault(x =>
				x.PackageName == packageName
			);

			var sb = new StringBuilder();
			sb.Append(pattern);
			sb
				.Replace("{Comments}", packageSchema.PackageName)
				.Replace("{Today}", now)
				.Replace("{Namespace}", nameSpace)
				.Replace("{PackageClassName}", packageClassName);

			return sb.ToString();
		}

		private string GeneratePackageSchema(string name)
		{
			var pattern =
@"// {PackageClassName}.cs
//
// Generated by Kuicker.Tools.EntityGenerator.
// {Today}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kuicker;
using Kuicker.Data;

namespace {Namespace}
{
	public partial class {PackageClassName} : IPackage
	{
		public static string __PackageName
		{
			get
			{
				return ""{PackageName}"";
			}
		}
{Procedures}
	}
}";
			var now = DateTime.Now.yyyy_MM_dd_HH_mm_ss_fff();
			var nameSpace = txtNamespace.Text;
			var packageName = name;
			var packageClassName = packageName
				.TrimStart(txtPackageTrimPrefix.Text);
			packageClassName = NamingFilter(packageClassName)
				.AppendSuffix(txtPackageAppendSuffix.Text);
			if(packageClassName.IsNullOrEmpty()) {
				packageClassName = packageName;
			}

			var packageSchema = PackageSchemas.FirstOrDefault(x =>
				x.PackageName == packageName
			);

			var ef = EnumCache.Get<DataFormat>();
			var sbProcedures = new StringBuilder();
			foreach(var procedure in packageSchema.Procedures) {
				var propertyName = procedure.ProcedureName
					.TrimStart(txtProcedureTrimPrefix.Text);
				propertyName = NamingFilter(propertyName);
				if(propertyName.IsNullOrEmpty()) {
					propertyName = procedure.ProcedureName;
				}

				if(sbProcedures.Length > 0) {
					sbProcedures.AppendLine();
				}

				sbProcedures.AppendFormat(@"
		#region {2}
		/// <summary>
		/// {0}
		/// </summary>
		public static {1} {2}({3})
		{{
			return {2}(
				new EntityApi(__PackageName){4}
			);
		}}

		public static {1} {2}(
			EntityApi api{5})
		{{
			return api.{6}(
				""{7}""{8}
			){9};
		}}
		#endregion",
					// 0
					procedure.DbFullName,
					// 1
					procedure.ToOutType(Formater),
					// 2
					propertyName,
					// 3
					(!procedure.Ins.IsNullOrEmpty()).If(
						procedure
							.Ins
							.Select(x => x.ToMethodArgument(Formater))
							.Join(
								"," + Environment.NewLine,
								"			",
								string.Empty
							)
							.AppendPrefix(Environment.NewLine)
					),
					// 4
					(!procedure.Ins.IsNullOrEmpty()).If(
						procedure
							.Ins
							.Select(x => x.ArgumentName)
							.Join(
								"," + Environment.NewLine,
								"				",
								string.Empty
							)
							.AppendPrefix("," + Environment.NewLine)
					),
					// 5
					(!procedure.Ins.IsNullOrEmpty()).If(
						procedure
							.Ins
							.Select(x => x.ToMethodArgument(Formater))
							.Join(
								"," + Environment.NewLine,
								"			",
								string.Empty
							)
							.AppendPrefix("," + Environment.NewLine)
					),
					// 6
					procedure.ToApiMethod(),
					// 7
					procedure.ProcedureName,
					// 8
					(!procedure.Ins.IsNullOrEmpty()).If(
						procedure
							.Ins
							.Select(x => string.Format(
								@"new Any(""{0}"", {0})", 
								x.ArgumentName
							))
							.Join(
								"," + Environment.NewLine,
								"				",
								string.Empty
							)
							.AppendPrefix("," + Environment.NewLine)
					),
					// 9
					procedure.ToOutFormatMethod(Formater)
				);
			}

			var sb = new StringBuilder();
			sb.Append(pattern);
			sb
				.Replace("{Comments}", packageSchema.PackageName)
				.Replace("{Today}", now)
				.Replace("{Namespace}", nameSpace)
				.Replace("{PackageClassName}", packageClassName)
				.Replace("{PackageName}", packageName)
				.Replace("{Procedures}", sbProcedures.ToString());

			return sb.ToString();
		}

		private void lbxPackages_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox lbx = sender as ListBox;
			if(null == lbx) { return; }

			clbProcedures.Items.Clear();
			txtCode.Text = string.Empty;
			string name = lbx.SelectedItem.ToString();
			if(name.IsNullOrEmpty()) { return; }

			var obj = PackageSchemas.FirstOrDefault(x => x.PackageName == name);
			if(null == obj) { return; }
			string procedurePrefix = string.Empty;
			bool procedurePrefixDone = false;
			foreach(var procedure in obj.Procedures) {
				clbProcedures.Items.Add(procedure.ProcedureName, true);

				if(!procedurePrefixDone) {
					if(procedurePrefix.IsNullOrEmpty()) {
						string[] parts = procedure.ProcedureName.Split('_');
						if(parts.Length > 1) {
							procedurePrefix = procedure.ProcedureName.Split('_')[0];
						} else {
							procedurePrefixDone = true;
						}
					} else {
						if(procedurePrefix != procedure.ProcedureName.Split('_')[0]) {
							procedurePrefix = string.Empty;
						} else {
							procedurePrefix += "_";
						}
						procedurePrefixDone = true;
					}
				}
			}

			RefreshPackage(name);
		}


		public bool Skip(string name)
		{
			if(null == _SkipPrefixes) {
				_SkipPrefixes = SplitPattern(txtSkipPrefix.Text);
			}

			return name.StartsX(_SkipPrefixes);
		}



		private string[] _SkipPrefixes;
		private string[] _Keywords;
		private string[] _Abbreviations;
		private string[] _Prefixes;
		private string[] _Suffixes;
		private string NamingFilter(string name)
		{
			if(null == _Keywords) {
				_Keywords = SplitPattern(txtKeyword.Text);
			}
			if(null == _Prefixes) {
				_Prefixes = SplitPattern(txtPrefix.Text);
			}
			if(null == _Suffixes) {
				_Suffixes = SplitPattern(txtSuffix.Text);
			}
			if(null == _Abbreviations) {
				_Abbreviations = SplitPattern(txtAbbreviation.Text);
			}

			if(name.Contains("_")) {
				var sb = new StringBuilder();
				foreach(var p in name.ToPopularCodeName().SplitAndTrim("_")) {
					var pp = p;
					bool found = false;
					foreach(var one in _Keywords) {
						if(one.Length == 1) { continue; }
						if(pp.ToLower() == one) {
							found = true;
							pp = one;
							break;
						}
					}
					if(!found) {
						pp = pp.ToPascalCasing();
					}
					sb.Append(pp);
				}
				return sb.ToString();
			}

			var formated = name.ToLower();

			foreach(var one in _Keywords) {
				if(one.Length == 1) { continue; }
				if(formated.IndexOf(one.ToLower()) > -1) {
					formated = formated
						.Replace(one.ToLower(), TempString(one));
				}
			}
			foreach(var one in _Prefixes) {
				if(one.Length == 1) { continue; }
				if(formated.StartsWith(one.ToLower())) {
					formated = formated
						.TrimStart(one)
						.AppendPrefix(TempString(one));
					break;
				}
			}
			foreach(var one in _Suffixes) {
				if(one.Length == 1) { continue; }
				if(formated.EndsWith(one.ToLower())) {
					formated = formated
						.TrimEnd(one)
						.AppendSuffix(TempString(one));
					break;
				}
			}
			foreach(var one in _Abbreviations) {
				if(one.Length == 1) { continue; }
				if(formated.IndexOf(one.ToLower()) > -1) {
					formated = formated
						.Replace(one.ToLower(), TempString(one));
				}
			}

			foreach(var alphabet in Constants.LowerAlphabet.ToCharArray()) {
				var ab = "~" + alphabet.ToString() + "~";
				if(formated.IndexOf(ab) > -1) {
					formated = formated
						.Replace(
							ab, 
							"~" + alphabet.ToString().ToUpper() + "~"
						);
				}

				var x = "~" + alphabet.ToString();
				if(formated.EndsWith(x)) {
					formated = formated
						.TrimEnd(x)
						.AppendSuffix("~" + alphabet.ToString().ToUpper());
				}
			}

			var list = new List<string>();
			foreach(var part in formated.SplitAndTrim("~")) {
				var x = part.Replace("!", "");
				if(x.IsNullOrEmpty()) { continue; }
				if(x.AllLower()) {
					list.Add(x.ToPascalCasing());
				} else {
					list.Add(x);
				}
			}

			return list
				.Join(string.Empty)
				.ToPopularCodeName();
		}

		private string TempString(string txt)
		{
			return "~" + txt.ToCharArray().ToStringArray().Join("!") + "~";
		}

		private string[] SplitPattern(string txt)
		{
			var patterns = txt.SplitAndTrim(",", " ");
			var list = new List<string>();
			foreach(var x in patterns) {
				if(x.IsNullOrWhiteSpace()) { continue; }
				if(x.AllLower()) {
					list.Add(x.ToPascalCasing());
				} else {
					list.Add(x);
				}
			}
			return list.ToArray();
		}
	}
}
