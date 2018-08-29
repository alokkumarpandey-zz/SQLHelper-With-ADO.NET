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
            List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
            Param.Add(new KeyValuePair<string, object>("@ProjectName", objInfo.ProjectName));
            Param.Add(new KeyValuePair<string, object>("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_AddProject";
            SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
            return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
        }

        public async Task<int> UpdateProject(ProjectInfo objInfo)
        {
            List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
            Param.Add(new KeyValuePair<string, object>("@ProjectID", objInfo.ProjectID));
            Param.Add(new KeyValuePair<string, object>("@ProjectName", objInfo.ProjectName));
            Param.Add(new KeyValuePair<string, object>("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_UpdateProject";
            SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
            return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
        }

        public async Task<ProjectInfo> GetProject(ProjectInfo objInfo)
        {
            List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
            Param.Add(new KeyValuePair<string, object>("@ProjectID", objInfo.ProjectID));            
            string strSpName = "usp_GetProject";
            SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
            //List<ProjectInfo> objList = await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName, Param);
            return await sqlHAsy.ExecuteAsObjectAsync<ProjectInfo>(strSpName, Param);
        }

        public async Task<List<ProjectInfo>> GetProjectList()
        {
            //List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
            //Param.Add(new KeyValuePair<string, object>("@ProjectID", objInfo.ProjectID));
            //Param.Add(new KeyValuePair<string, object>("@ProjectName", objInfo.ProjectName));
            //Param.Add(new KeyValuePair<string, object>("@ProjectCode", objInfo.ProjectCode));
            string strSpName = "usp_GetProjectList";
            SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
            //List<ProjectInfo> objList = await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName, Param);
            return await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName);
        }
    }

}
