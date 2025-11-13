## 스파르타유니티_개인과제_던전만들기_김주원

### 1. 개요
유니티 개임 개발 숙련 주차 개인과제입니다.<br>
3D 플랫폼에서 이동, 점프 등의 조작을 통한 던전 제작을 목표로 하고 있습니다.<br>

### 2. 기능설명

  wasd를 이용한 캐릭터 이동<br>
  <img width="806" height="447" alt="화면 캡처 2025-11-13 152153" src="https://github.com/user-attachments/assets/4b43b5f7-af22-44e5-a768-57df4fb04a85" />
  
  스페이스바로 점프<br>
  <img width="801" height="448" alt="화면 캡처 2025-11-13 152215" src="https://github.com/user-attachments/assets/e3cddb23-f5a0-4d05-a679-0e1a569c102f" />

  키보드 c를 통한 카메라 전환(3인칭)<br>
  <img width="803" height="450" alt="화면 캡처 2025-11-13 152248" src="https://github.com/user-attachments/assets/6d32bc41-dfe8-4f0b-8528-898a7d254af9" />
  
  키보드 c를 통한 카메라 전환(1인칭)<br>
  <img width="802" height="450" alt="화면 캡처 2025-11-13 152312" src="https://github.com/user-attachments/assets/bd4fca55-b6d7-40df-ac7f-3b172bb79b18" />

  벽타기 기능<br>
  벽에 븥은 상태에서 점프 시 이동방식 변화(상하좌우, 중력 영향 없음)<br>
  벽타기 상태에서는 3인칭으로 전환, 캐릭터 회전 없이 카메라만 회전<br>
  캐릭터 기준 상하좌우에서 전방으로 레이캐스트, 벽 레이어 오브젝트 검사를 통한 벽타기 상태 결정<br>
  <img width="803" height="446" alt="화면 캡처 2025-11-13 152418" src="https://github.com/user-attachments/assets/31aa4c09-d1af-45cd-8df2-bba3f3c4137d" />

  이동 발판<br>
  플레이어가 위쪽 방향에서 충돌 중일 경우 발판의 이동속도를 계산하여 플레이어에게 전달<br>
  <img width="803" height="451" alt="화면 캡처 2025-11-13 152518" src="https://github.com/user-attachments/assets/656b573e-c51b-47ba-85f7-a666557ef136" />

  점프 발판<br>
  플레이어 충돌 시 플레이어에게 위쪽방향으로 힘을 가해 점프를 구현<br>
  <img width="803" height="451" alt="화면 캡처 2025-11-13 152643" src="https://github.com/user-attachments/assets/3813c417-d0d5-47de-b367-00e8162032a1" />

  레이저 트랩<br>
  플레이어 충돌 시 플레이어에게 데미지를 주는 레이저<br>
  시작 노드와 끝 노드 2개를 배치하고 레이저가 두 노드를 잇도록 좌표, 길이, 회전 값 계산<br>
  충돌 중일 경우 지속적으로 데미지를 줌<br>
  <img width="803" height="448" alt="화면 캡처 2025-11-13 152712" src="https://github.com/user-attachments/assets/6979953c-e622-496a-84c6-028a380fbb6e" />

  <img width="805" height="453" alt="화면 캡처 2025-11-13 152731" src="https://github.com/user-attachments/assets/5531e818-eee6-43ad-97ff-5e628fc2b3ec" />

  발사대<br>
  플레이어 충돌 시 바닥의 원형 게이지가 차오르며 전부 차면 지정해둔 방향과 힘으로 플레이어를 날림<br>
  상단 화살표는 지정해둔 방향에 맞게 회전값 계산하여 표시<br>
  <img width="804" height="451" alt="화면 캡처 2025-11-13 152810" src="https://github.com/user-attachments/assets/08c790a3-20bb-4c62-8526-b8892f57f145" />
  
  <img width="802" height="448" alt="화면 캡처 2025-11-13 152922" src="https://github.com/user-attachments/assets/528cc759-28de-40f2-83ac-136ec90baaad" />
  











