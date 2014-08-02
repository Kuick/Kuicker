namespace Kuicker.Tools.EntityGenerator
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbxTables = new System.Windows.Forms.ListBox();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.btnReflesh = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTableTrimPrefix = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtTableAppendSuffix = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.clbColumns = new System.Windows.Forms.CheckedListBox();
			this.txtColumnTrimPrefix = new System.Windows.Forms.TextBox();
			this.txtColumnAppendSuffix = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.cbbProviders = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.btnOutput = new System.Windows.Forms.Button();
			this.txtOutputPath = new System.Windows.Forms.TextBox();
			this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.btnSelectPath = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtArgumentTrimPrefix = new System.Windows.Forms.TextBox();
			this.txtProcedureTrimPrefix = new System.Windows.Forms.TextBox();
			this.txtPackageTrimPrefix = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.txtPackageAppendSuffix = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lbxPackages = new System.Windows.Forms.ListBox();
			this.label13 = new System.Windows.Forms.Label();
			this.clbProcedures = new System.Windows.Forms.CheckedListBox();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.txtAbbreviation = new System.Windows.Forms.TextBox();
			this.txtSuffix = new System.Windows.Forms.TextBox();
			this.txtKeyword = new System.Windows.Forms.TextBox();
			this.txtPrefix = new System.Windows.Forms.TextBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label26 = new System.Windows.Forms.Label();
			this.txtSkipPrefix = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Connection String";
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtConnectionString.Location = new System.Drawing.Point(116, 40);
			this.txtConnectionString.Multiline = true;
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtConnectionString.Size = new System.Drawing.Size(198, 48);
			this.txtConnectionString.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(15, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Tables && Views";
			// 
			// lbxTables
			// 
			this.lbxTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbxTables.FormattingEnabled = true;
			this.lbxTables.ItemHeight = 15;
			this.lbxTables.Location = new System.Drawing.Point(18, 34);
			this.lbxTables.Name = "lbxTables";
			this.lbxTables.Size = new System.Drawing.Size(151, 94);
			this.lbxTables.TabIndex = 3;
			this.lbxTables.SelectedIndexChanged += new System.EventHandler(this.lbxTables_SelectedIndexChanged);
			// 
			// txtCode
			// 
			this.txtCode.AcceptsReturn = true;
			this.txtCode.AcceptsTab = true;
			this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCode.HideSelection = false;
			this.txtCode.Location = new System.Drawing.Point(12, 549);
			this.txtCode.Multiline = true;
			this.txtCode.Name = "txtCode";
			this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtCode.Size = new System.Drawing.Size(705, 96);
			this.txtCode.TabIndex = 5;
			// 
			// btnReflesh
			// 
			this.btnReflesh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReflesh.Location = new System.Drawing.Point(116, 94);
			this.btnReflesh.Name = "btnReflesh";
			this.btnReflesh.Size = new System.Drawing.Size(80, 29);
			this.btnReflesh.TabIndex = 4;
			this.btnReflesh.Text = "Refresh";
			this.btnReflesh.UseVisualStyleBackColor = true;
			this.btnReflesh.Click += new System.EventHandler(this.btnReflesh_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(47, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 15);
			this.label3.TabIndex = 2;
			this.label3.Text = "Table";
			// 
			// txtTableTrimPrefix
			// 
			this.txtTableTrimPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTableTrimPrefix.Location = new System.Drawing.Point(153, 47);
			this.txtTableTrimPrefix.Name = "txtTableTrimPrefix";
			this.txtTableTrimPrefix.Size = new System.Drawing.Size(54, 21);
			this.txtTableTrimPrefix.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(229, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 15);
			this.label4.TabIndex = 2;
			this.label4.Text = "Append Suffix";
			// 
			// txtTableAppendSuffix
			// 
			this.txtTableAppendSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTableAppendSuffix.Location = new System.Drawing.Point(317, 48);
			this.txtTableAppendSuffix.Name = "txtTableAppendSuffix";
			this.txtTableAppendSuffix.Size = new System.Drawing.Size(54, 21);
			this.txtTableAppendSuffix.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(172, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 15);
			this.label5.TabIndex = 2;
			this.label5.Text = "Columns";
			// 
			// clbColumns
			// 
			this.clbColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clbColumns.FormattingEnabled = true;
			this.clbColumns.Location = new System.Drawing.Point(175, 34);
			this.clbColumns.Name = "clbColumns";
			this.clbColumns.Size = new System.Drawing.Size(151, 100);
			this.clbColumns.TabIndex = 6;
			// 
			// txtColumnTrimPrefix
			// 
			this.txtColumnTrimPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtColumnTrimPrefix.Location = new System.Drawing.Point(153, 77);
			this.txtColumnTrimPrefix.Name = "txtColumnTrimPrefix";
			this.txtColumnTrimPrefix.Size = new System.Drawing.Size(54, 21);
			this.txtColumnTrimPrefix.TabIndex = 1;
			// 
			// txtColumnAppendSuffix
			// 
			this.txtColumnAppendSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtColumnAppendSuffix.Location = new System.Drawing.Point(317, 75);
			this.txtColumnAppendSuffix.Name = "txtColumnAppendSuffix";
			this.txtColumnAppendSuffix.Size = new System.Drawing.Size(54, 21);
			this.txtColumnAppendSuffix.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(86, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(66, 15);
			this.label6.TabIndex = 2;
			this.label6.Text = "Trim Prefix";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(229, 80);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 15);
			this.label7.TabIndex = 2;
			this.label7.Text = "Append Suffix";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(9, 22);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 15);
			this.label8.TabIndex = 2;
			this.label8.Text = "Namespace";
			// 
			// txtNamespace
			// 
			this.txtNamespace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtNamespace.Location = new System.Drawing.Point(89, 19);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(282, 21);
			this.txtNamespace.TabIndex = 1;
			// 
			// cbbProviders
			// 
			this.cbbProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbProviders.FormattingEnabled = true;
			this.cbbProviders.Location = new System.Drawing.Point(116, 11);
			this.cbbProviders.Name = "cbbProviders";
			this.cbbProviders.Size = new System.Drawing.Size(198, 23);
			this.cbbProviders.TabIndex = 7;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(29, 14);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(52, 15);
			this.label10.TabIndex = 0;
			this.label10.Text = "Provider";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(13, 522);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(71, 15);
			this.label11.TabIndex = 0;
			this.label11.Text = "Output Path";
			// 
			// btnOutput
			// 
			this.btnOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOutput.Location = new System.Drawing.Point(637, 516);
			this.btnOutput.Name = "btnOutput";
			this.btnOutput.Size = new System.Drawing.Size(80, 27);
			this.btnOutput.TabIndex = 4;
			this.btnOutput.Text = "Output";
			this.btnOutput.UseVisualStyleBackColor = true;
			this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
			// 
			// txtOutputPath
			// 
			this.txtOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtOutputPath.Location = new System.Drawing.Point(90, 519);
			this.txtOutputPath.Name = "txtOutputPath";
			this.txtOutputPath.Size = new System.Drawing.Size(348, 21);
			this.txtOutputPath.TabIndex = 1;
			// 
			// btnSelectPath
			// 
			this.btnSelectPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelectPath.Location = new System.Drawing.Point(551, 516);
			this.btnSelectPath.Name = "btnSelectPath";
			this.btnSelectPath.Size = new System.Drawing.Size(80, 27);
			this.btnSelectPath.TabIndex = 4;
			this.btnSelectPath.Text = "Select Path";
			this.btnSelectPath.UseVisualStyleBackColor = true;
			this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(86, 52);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 15);
			this.label9.TabIndex = 2;
			this.label9.Text = "Trim Prefix";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(33, 80);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(50, 15);
			this.label12.TabIndex = 2;
			this.label12.Text = "Column";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cbbProviders);
			this.groupBox1.Controls.Add(this.txtConnectionString);
			this.groupBox1.Controls.Add(this.btnReflesh);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(321, 136);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtArgumentTrimPrefix);
			this.groupBox2.Controls.Add(this.txtProcedureTrimPrefix);
			this.groupBox2.Controls.Add(this.txtPackageTrimPrefix);
			this.groupBox2.Controls.Add(this.txtTableTrimPrefix);
			this.groupBox2.Controls.Add(this.txtTableAppendSuffix);
			this.groupBox2.Controls.Add(this.txtColumnTrimPrefix);
			this.groupBox2.Controls.Add(this.label20);
			this.groupBox2.Controls.Add(this.txtPackageAppendSuffix);
			this.groupBox2.Controls.Add(this.txtColumnAppendSuffix);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.txtNamespace);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label21);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label19);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Location = new System.Drawing.Point(339, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(378, 199);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Naming 1";
			// 
			// txtArgumentTrimPrefix
			// 
			this.txtArgumentTrimPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtArgumentTrimPrefix.Location = new System.Drawing.Point(153, 158);
			this.txtArgumentTrimPrefix.Name = "txtArgumentTrimPrefix";
			this.txtArgumentTrimPrefix.Size = new System.Drawing.Size(54, 21);
			this.txtArgumentTrimPrefix.TabIndex = 1;
			// 
			// txtProcedureTrimPrefix
			// 
			this.txtProcedureTrimPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtProcedureTrimPrefix.Location = new System.Drawing.Point(153, 131);
			this.txtProcedureTrimPrefix.Name = "txtProcedureTrimPrefix";
			this.txtProcedureTrimPrefix.Size = new System.Drawing.Size(54, 21);
			this.txtProcedureTrimPrefix.TabIndex = 1;
			// 
			// txtPackageTrimPrefix
			// 
			this.txtPackageTrimPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPackageTrimPrefix.Location = new System.Drawing.Point(153, 104);
			this.txtPackageTrimPrefix.Name = "txtPackageTrimPrefix";
			this.txtPackageTrimPrefix.Size = new System.Drawing.Size(54, 21);
			this.txtPackageTrimPrefix.TabIndex = 1;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label20.Location = new System.Drawing.Point(86, 163);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(66, 15);
			this.label20.TabIndex = 2;
			this.label20.Text = "Trim Prefix";
			// 
			// txtPackageAppendSuffix
			// 
			this.txtPackageAppendSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPackageAppendSuffix.Location = new System.Drawing.Point(317, 104);
			this.txtPackageAppendSuffix.Name = "txtPackageAppendSuffix";
			this.txtPackageAppendSuffix.Size = new System.Drawing.Size(54, 21);
			this.txtPackageAppendSuffix.TabIndex = 1;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(86, 136);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(66, 15);
			this.label18.TabIndex = 2;
			this.label18.Text = "Trim Prefix";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(86, 109);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(66, 15);
			this.label17.TabIndex = 2;
			this.label17.Text = "Trim Prefix";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.Location = new System.Drawing.Point(229, 104);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(82, 15);
			this.label21.TabIndex = 2;
			this.label21.Text = "Append Suffix";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.Location = new System.Drawing.Point(19, 164);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(60, 15);
			this.label19.TabIndex = 2;
			this.label19.Text = "Argument";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(19, 134);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(64, 15);
			this.label16.TabIndex = 2;
			this.label16.Text = "Procedure";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(28, 107);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(55, 15);
			this.label15.TabIndex = 2;
			this.label15.Text = "Package";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.lbxPackages);
			this.groupBox3.Controls.Add(this.lbxTables);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.clbProcedures);
			this.groupBox3.Controls.Add(this.clbColumns);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Location = new System.Drawing.Point(12, 217);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(705, 154);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Schema";
			// 
			// lbxPackages
			// 
			this.lbxPackages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbxPackages.FormattingEnabled = true;
			this.lbxPackages.ItemHeight = 15;
			this.lbxPackages.Location = new System.Drawing.Point(332, 34);
			this.lbxPackages.Name = "lbxPackages";
			this.lbxPackages.Size = new System.Drawing.Size(151, 94);
			this.lbxPackages.TabIndex = 3;
			this.lbxPackages.SelectedIndexChanged += new System.EventHandler(this.lbxPackages_SelectedIndexChanged);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(329, 16);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(61, 15);
			this.label13.TabIndex = 2;
			this.label13.Text = "Packages";
			// 
			// clbProcedures
			// 
			this.clbProcedures.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clbProcedures.FormattingEnabled = true;
			this.clbProcedures.Location = new System.Drawing.Point(489, 34);
			this.clbProcedures.Name = "clbProcedures";
			this.clbProcedures.Size = new System.Drawing.Size(151, 100);
			this.clbProcedures.TabIndex = 6;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(486, 16);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(70, 15);
			this.label14.TabIndex = 2;
			this.label14.Text = "Procedures";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label24);
			this.groupBox4.Controls.Add(this.label25);
			this.groupBox4.Controls.Add(this.label23);
			this.groupBox4.Controls.Add(this.label22);
			this.groupBox4.Controls.Add(this.txtAbbreviation);
			this.groupBox4.Controls.Add(this.txtSuffix);
			this.groupBox4.Controls.Add(this.txtKeyword);
			this.groupBox4.Controls.Add(this.txtPrefix);
			this.groupBox4.Location = new System.Drawing.Point(12, 378);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(702, 132);
			this.groupBox4.TabIndex = 12;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Naming 2";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label24.Location = new System.Drawing.Point(329, 16);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(37, 15);
			this.label24.TabIndex = 0;
			this.label24.Text = "Suffix";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label25.Location = new System.Drawing.Point(486, 16);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(74, 15);
			this.label25.TabIndex = 0;
			this.label25.Text = "Abbreviation";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.Location = new System.Drawing.Point(15, 16);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(54, 15);
			this.label23.TabIndex = 0;
			this.label23.Text = "Keyword";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.Location = new System.Drawing.Point(172, 16);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(38, 15);
			this.label22.TabIndex = 0;
			this.label22.Text = "Prefix";
			// 
			// txtAbbreviation
			// 
			this.txtAbbreviation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtAbbreviation.Location = new System.Drawing.Point(489, 34);
			this.txtAbbreviation.Multiline = true;
			this.txtAbbreviation.Name = "txtAbbreviation";
			this.txtAbbreviation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAbbreviation.Size = new System.Drawing.Size(151, 92);
			this.txtAbbreviation.TabIndex = 1;
			// 
			// txtSuffix
			// 
			this.txtSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSuffix.Location = new System.Drawing.Point(332, 34);
			this.txtSuffix.Multiline = true;
			this.txtSuffix.Name = "txtSuffix";
			this.txtSuffix.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSuffix.Size = new System.Drawing.Size(151, 92);
			this.txtSuffix.TabIndex = 1;
			// 
			// txtKeyword
			// 
			this.txtKeyword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtKeyword.Location = new System.Drawing.Point(18, 34);
			this.txtKeyword.Multiline = true;
			this.txtKeyword.Name = "txtKeyword";
			this.txtKeyword.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtKeyword.Size = new System.Drawing.Size(151, 92);
			this.txtKeyword.TabIndex = 1;
			// 
			// txtPrefix
			// 
			this.txtPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrefix.Location = new System.Drawing.Point(175, 34);
			this.txtPrefix.Multiline = true;
			this.txtPrefix.Name = "txtPrefix";
			this.txtPrefix.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtPrefix.Size = new System.Drawing.Size(151, 92);
			this.txtPrefix.TabIndex = 1;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.label26);
			this.groupBox5.Controls.Add(this.txtSkipPrefix);
			this.groupBox5.Location = new System.Drawing.Point(12, 155);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(321, 56);
			this.groupBox5.TabIndex = 13;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Filter";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label26.Location = new System.Drawing.Point(6, 16);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(65, 15);
			this.label26.TabIndex = 2;
			this.label26.Text = "Skip Prefix";
			// 
			// txtSkipPrefix
			// 
			this.txtSkipPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSkipPrefix.Location = new System.Drawing.Point(78, 13);
			this.txtSkipPrefix.Name = "txtSkipPrefix";
			this.txtSkipPrefix.Size = new System.Drawing.Size(236, 21);
			this.txtSkipPrefix.TabIndex = 1;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(726, 657);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtCode);
			this.Controls.Add(this.btnSelectPath);
			this.Controls.Add(this.btnOutput);
			this.Controls.Add(this.txtOutputPath);
			this.Controls.Add(this.label11);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Text = "Kuicker Entity Generator";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbxTables;
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.Button btnReflesh;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTableTrimPrefix;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtTableAppendSuffix;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckedListBox clbColumns;
		private System.Windows.Forms.TextBox txtColumnTrimPrefix;
		private System.Windows.Forms.TextBox txtColumnAppendSuffix;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtNamespace;
		private System.Windows.Forms.ComboBox cbbProviders;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button btnOutput;
		private System.Windows.Forms.TextBox txtOutputPath;
		private System.Windows.Forms.FolderBrowserDialog fbDialog;
		private System.Windows.Forms.Button btnSelectPath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ListBox lbxPackages;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckedListBox clbProcedures;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txtArgumentTrimPrefix;
		private System.Windows.Forms.TextBox txtProcedureTrimPrefix;
		private System.Windows.Forms.TextBox txtPackageTrimPrefix;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtPackageAppendSuffix;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.TextBox txtSuffix;
		private System.Windows.Forms.TextBox txtKeyword;
		private System.Windows.Forms.TextBox txtPrefix;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.TextBox txtAbbreviation;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox txtSkipPrefix;
	}
}

