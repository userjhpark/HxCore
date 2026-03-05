using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HxCore
{
    /// <summary>
    /// Database Column명을 가져 올때 변환 타입
    /// </summary>
    [TypeConverterAttribute(typeof(HxEnumConverter)), DefaultValue(HxDbColumnNameCharType.Original)]
    public enum HxDbColumnNameCharType
    {
        /// <summary>
        /// Original
        /// </summary>
        Original = 0,
        /// <summary>
        /// To Lower
        /// </summary>
        Lower,
        /// <summary>
        /// To Upper
        /// </summary>
        Upper
    }
    /// <summary>
    /// Database Sequence 적용 타입 
    /// </summary>
    [TypeConverterAttribute(typeof(HxEnumConverter)), DefaultValue(HxDbSeqModeType.Auto)]
    public enum HxDbSeqModeType
    {
        /// <summary>
        /// Auto
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Use Sequence Table(ex : db_sequence)
        /// </summary>
        UseTable = 1,
        /// <summary>
        /// Use Oracle Sequence Object
        /// </summary>
        UseOracleSequence = 2
    }
    public enum HxDbSqlModeType
    {
        Select,
        Insert,
        Update,
        Delete,
        Merge,
        Package,
        Procedure,
        Function,
        Sequence,
        Trigger,
        Synonym,
        JobScheduler
    }

    /// <summary>
    /// Application Oracle Service 연결 타입
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(HxEnumConverter))]
    public enum HxDbOracleConnectionType
    {
        [Description("미 지정")]
        None,
        [Description("[Oracle]TNS Name Service")]
        TNS,
        [Description("[Oracle]Direct Connection")]
        Direct
    }

    public enum HxDbParamDirectionType //ParameterDirection
    {
        //
        // 요약:
        //     The parameter is an input parameter.
        Input = 1,
        //
        // 요약:
        //     The parameter is an output parameter.
        Output = 2,
        //
        // 요약:
        //     The parameter is capable of both input and output.
        InputOutput = 3,
        //
        // 요약:
        //     The parameter represents a return value from an operation such as a stored procedure,
        //     built-in function, or user-defined function.
        ReturnValue = 6
    }
    public enum HxDbParamValueType
    {
        Default,
        //Input,
        //Param,
        Date,
        Time,
        DateTime,
        UnixTime,
        CLOB,
        BLOB,
        None = Default
    }

    /// <summary>
    /// Database Column명을 가져 올때 변환 타입
    /// </summary>
    [TypeConverterAttribute(typeof(HxEnumConverter)), DefaultValue(HxDbObjectType.SelectOnlyObjects)]
    public enum HxDbObjectType
    {
        /// <summary>
        /// Select Only Object (Table, View)
        /// </summary>
        SelectOnlyObjects = (HxDbObjectType.Table | HxDbObjectType.View),
        /// <summary>
        /// Table
        /// </summary>
        Table = 1 << 0,
        /// <summary>
        /// View
        /// </summary>
        View = 1 << 2,
        /// <summary>
        /// Synonym(Alias)
        /// </summary>
        Synonym = 1 << 3,
        /// <summary>
        /// Sequence(Auto Increment)
        /// </summary>
        Sequence = 1 << 4
        /*,
    /// <summary>
    /// Trigger
    /// </summary>
    Trigger,
    /// <summary>
    /// Index
    /// </summary>
    Index
    */

    }
}
