# SantyTime
![Title](https://user-images.githubusercontent.com/26218409/165750404-9f2565eb-e1bb-457f-add6-4a10696ff999.png)


=======================================================================

* 2인개발 프로젝트 (김준연/정연희)
* 사용 툴 : 유니티, visual studio 2017
* 모바일 감성 리듬게임

=======================================================================

# 인게임 
![select](https://user-images.githubusercontent.com/26218409/165755035-f2091050-4168-47ed-8b5d-57f8e184a5f1.png)
![loading](https://user-images.githubusercontent.com/26218409/165755037-61c509c8-3b31-40d3-924b-cd5e3302de03.png)
![pause](https://user-images.githubusercontent.com/26218409/165755046-824eaf47-9626-494b-9013-70e7a30a0801.png)
![play](https://user-images.githubusercontent.com/26218409/165755047-47672d42-d7a1-4fa3-8c5c-6cacc3f10888.png)
![result-1](https://user-images.githubusercontent.com/26218409/165755051-cb74702c-655c-4294-8848-0ebd589f196f.png)
![result-2](https://user-images.githubusercontent.com/26218409/165755053-ef4141ab-2ea9-432e-a71e-3e529fa7ceee.png)

# 뼈대작업
* json 포맷으로 노트 등장시간, 방향을 관리

![note_info](https://user-images.githubusercontent.com/26218409/165755817-1c962063-0b8e-4ff3-97f9-bb19f961b1c6.png)

* json 데이터를 로드하고 음악의 진행시간을 따라가며 노트 생성

![note_creat](https://user-images.githubusercontent.com/26218409/165756685-36e1dc46-8d43-4d01-bd14-a3a552e530ca.png)


    IEnumerator CreatNote(float StreamingSec)
    {
        //MusicPlayer           //PlayerManager
        MP.PlayMusic(stages_song[PM.stagenum]);

                            //ObjectCnt = JD["Notes"].Count;
        for (int i = 0; i < ObjectCnt; i++)
        {
            actualNoteCnt = JD["Notes"][i]["Index"].Count;
            hitTime = float.Parse(JD["Notes"][i]["HitTime"].ToString());

            yield return new WaitUntil(() => musicTime >= hitTime-1f);

            for (int j = 0; j < actualNoteCnt; j++)
            {
                yield return new WaitUntil(() => PM.isPlaying);
                GameObject newNote = Instantiate(Note, new Vector3(0,-30,0), Quaternion.identity);
                newNote.transform.SetParent(ParantNote.transform);
                newNote.transform.localScale = new Vector3(1,1,1);
                newNote.transform.localPosition = new Vector3(0, -30, 0);

                newNote.GetComponent<Notes>().SetNote(int.Parse(JD["Notes"][i]["Index"][j]["Path"].ToString()), notespeed);
                total_notes++;
            }
        }

        yield return new WaitUntil(()=> !MP.AS.isPlaying);
        PM.EndSong(total_notes);
    }
    '''

# 기타작업
* UI, 사운드 애니메이션 등
# 기획서
[plan.pdf](https://github.com/mynamejohn/SantyTime/files/8582676/plan.pdf)
