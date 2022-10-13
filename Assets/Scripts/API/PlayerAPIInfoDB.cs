
[System.Serializable]
public class PlayerAPIInfoDB
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // API 호출 상태 코드 200이 아니면 에러를 반환한다.
    public string statusCode;

    // 매번 바뀌는 세션 ID
    public string sessionId;

    // 유저 고유 이름
    public string userName;

    // 유저 고유 ID
    public string _id;

    // 유저의 재화를 저장할 변수
    public string zera;
    public string ace;

    // Post 메서드 호출 시, "success"
    public string message;


}
