using SQLHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Project.Models
{
    public class GenralCRUD
    {
        public async Task<int> AddProject(ProjectInfo objInfo)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@ProjectName", objInfo.ProjectName));
            Param.Add(new SQLParam("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_AddProject";
            SQLExecuteNonQueryAsync sqlHAsy = new SQLExecuteNonQueryAsync();
            return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
        }
        public async Task<int> UpdateProject(ProjectInfo objInfo)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@ProjectID", objInfo.ProjectID));
            Param.Add(new SQLParam("@ProjectName", objInfo.ProjectName));
            Param.Add(new SQLParam("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_UpdateProject";
            SQLExecuteNonQueryAsync sqlHAsy = new SQLExecuteNonQueryAsync();
            return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
        }
        public async Task<ProjectInfo> GetProject(ProjectInfo objInfo)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@ProjectID", objInfo.ProjectID));
            string strSpName = "usp_GetProject";
            SQLGetAsync sqlHAsy = new SQLGetAsync();
            //List<ProjectInfo> objList = await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName, Param);
            return await sqlHAsy.ExecuteAsObjectAsync<ProjectInfo>(strSpName, Param);
        }
        public async Task<IList<ProjectInfo>> GetProjectList()
        {
            //List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
            //Param.Add(new KeyValuePair<string, object>("@ProjectID", objInfo.ProjectID));
            //Param.Add(new KeyValuePair<string, object>("@ProjectName", objInfo.ProjectName));
            //Param.Add(new KeyValuePair<string, object>("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_GetProjectList";
            SQLGetListAsync sqlHAsy = new SQLGetListAsync();
            //List<ProjectInfo> objList = await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName, Param);
            return await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName);
        }
    }

}
