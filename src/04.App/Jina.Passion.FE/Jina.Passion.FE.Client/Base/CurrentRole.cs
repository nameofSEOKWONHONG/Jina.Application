namespace Jina.Passion.FE.Client.Base
{
    public class CurrentRole
    {
        /// <summary>
        /// 수정권한
        /// </summary>
        protected bool IsEdit { get; set; }

        /// <summary>
        /// 삭제권한
        /// </summary>
        protected bool IsDelete { get; set; }

        /// <summary>
        /// 검색권한
        /// </summary>
        protected bool IsSearch { get; set; }

        /// <summary>
        /// 조회권한
        /// </summary>
        protected bool IsView { get; set; }

        /// <summary>
        /// 생성권한
        /// </summary>
        protected bool IsCreate { get; set; }

        /// <summary>
        /// 엑셀 추출권한
        /// </summary>
        protected bool IsExport { get; set; }

        /// <summary>
        /// 엑셀 업로드권한
        /// </summary>
        protected bool IsImport { get; set; }

        /// <summary>
        /// 접속자 관리자 권한
        /// </summary>
        protected bool IsAdmin { get; set; }
    }
}