using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore.Data
{
    using System.Data;
    using TFacoty = System.Data.Common.DbProviderFactory;
    using TConnection = System.Data.Common.DbConnection;
    using TTransaction = System.Data.Common.DbTransaction;
    using TCommand = System.Data.Common.DbCommand;
    using TParameter = System.Data.Common.DbParameter;
    using TDataReader = System.Data.Common.DbDataReader;
    using TDataAdapter = System.Data.Common.DbDataAdapter;
    using HxCore;

    public class HxDbCommon : HxDbA<TFacoty, TConnection, TTransaction, TCommand, TParameter, TDataReader, TDataAdapter>
    {
        public override string GetName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            //throw new NotImplementedException();
        }
        public override string ParamterSeparatorChar => "@";
        #region Static Intance
        private static HxDbCommon _instance = null;
        static HxDbCommon()
        {
            _instance = new HxDbCommon(HxDbProviderType.Common);
        }

        

        /// <summary>
        /// [Static]Instance Object
        /// </summary>
        public static HxDbCommon Instance
        {
            get { return _instance ?? (_instance = new HxDbCommon(HxDbProviderType.Common)); }
            private set { _instance = value; }
        }
        #endregion

        public static HxDbCommon Create(HxDbProviderType providerType = HxDbProviderType.Common)
        {
            return new HxDbCommon(providerType);
        }

        public override void SetOptions(HxDbOptionRec option)
        {
            throw new NotImplementedException();
        }

        protected override void InitVarTypes()
        {
            throw new NotImplementedException();
        }

        public override int GetRowCount(int parse = -1)
        {
            throw new NotImplementedException();
        }

        public override DataTable UserTables()
        {
            throw new NotImplementedException();
        }

        public override DataTable UserColumns()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string name, HxDbObjectType objectType = HxDbObjectType.SelectOnlyObjects)
        {
            throw new NotImplementedException();
        }

        public override bool TableContains(string name)
        {
            throw new NotImplementedException();
        }

        public override bool ViewContains(string name)
        {
            throw new NotImplementedException();
        }

        public override bool SynonymContains(string name)
        {
            throw new NotImplementedException();
        }

        public override bool SequenceContains(string name)
        {
            throw new NotImplementedException();
        }

        public override bool ColumnContains(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public override string NowDateValue(string dateFormatString = null)
        {
            throw new NotImplementedException();
        }

        protected override DataTable QueryStoredProcedureDataTable(string queryString, TParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public HxDbCommon(HxDbProviderType providerType) : base(providerType)
        {
        }

        public HxDbCommon(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null) : base(providerType, userID, password, database, character, pooling)
        {
        }

        public HxDbCommon(HxDbProviderType providerType, string userID, string password, string database, HxDbOptionRec option) : base(providerType, userID, password, database, option)
        {
        }

        public HxDbCommon(HxDbProviderType providerType, string connectionString, HxDbOptionRec option = default) : base(providerType, connectionString, option)
        {
        }

        public HxDbCommon(HxDbConnectionRec connection) : base(connection)
        {
        }

        public HxDbCommon(TConnection connectionResource) : base(connectionResource)
        {
        }
    }

}
