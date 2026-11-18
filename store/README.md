# Microsoft Store 등록 절차

이 폴더는 ThePen을 MSIX 패키지로 만들어 Microsoft Store에 올리기 위한 파일입니다.

| 파일 | 내용 |
|---|---|
| `AppxManifest.template.xml` | 패키지 매니페스트 틀 |
| `Assets/` | 스토어·시작 메뉴·작업표시줄용 로고 (`ThePen/Template.ico`에서 생성) |
| `build-msix.ps1` | 게시 → 매니페스트 채우기 → 패키지 생성 |
| `listing.md` | 스토어 등록 정보 초안 (영어·한국어) |
| `out/` | 빌드 결과 (git에 올리지 않음) |

## 1. 개발자 계정 만들기
 1. https://partner.microsoft.com/dashboard 에 Microsoft 계정으로 로그인합니다.
 2. "개인" 개발자 계정으로 등록합니다.

## 2. 앱 이름 예약
 1. Partner Center → 앱 및 게임 → 새 제품 → MSIX 또는 PWA 앱.
 2. 이름 `ThePen`을 예약합니다. 이미 사용 중이면 다른 이름을 골라야 합니다.

## 3. 제품 ID 값 확인
제품 관리 → **제품 ID**(Product identity)에서 세 값을 확인합니다.

 * `Package/Identity/Name` (예: `12345ladofa.ThePen`)
 * `Package/Identity/Publisher` (예: `CN=XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX`)
 * `Package/Properties/PublisherDisplayName`

## 4. 패키지 만들기
저장소 루트에서 실행합니다. 세 값은 3번에서 확인한 것과 정확히 같아야 합니다.

```powershell
.\store\build-msix.ps1 -IdentityName "12345ladofa.ThePen" -Publisher "CN=XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX" -PublisherDisplayName "ladofa" -Version 1.0.0.0
```

 * 결과 : `store\out\ThePen_1.0.0.0_x64.msix`
 * 서명은 하지 않습니다. 제출하면 스토어가 서명합니다.
 * 버전은 `1.2.3.0` 형식이어야 하고, 새로 올릴 때마다 올려야 합니다.

## 5. 제출
 1. 패키지 : 4번의 `.msix` 파일을 올립니다.
 2. 속성 : 범주, 개인정보처리방침 URL (`listing.md` 참고).
 3. 연령 등급 : 설문 작성.
 4. 스토어 등록 정보 : `listing.md`의 내용과 스크린샷.
 5. 제출 옵션 → **제한된 기능** 설명란에 `runFullTrust` 사유를 적습니다.
    > ThePen is a classic WPF desktop application. It needs full trust to show a transparent drawing window over the whole screen and to receive global mouse input and hot keys.
 6. 인증에 제출합니다. 심사는 보통 며칠 걸립니다.

## 설치 전 내 PC에서 시험하기 (선택)
서명되지 않은 패키지는 바로 설치할 수 없습니다. Windows 설정에서 개발자 모드를 켠 뒤, 풀어 놓은 폴더를 등록하면 스토어 설치와 같은 조건으로 실행해 볼 수 있습니다.

```powershell
Add-AppxPackage -Register .\store\out\layout\AppxManifest.xml
```

시험이 끝나면 시작 메뉴에서 ThePen을 제거합니다.

## 사용자 파일 위치
스토어 설치본은 프로그램 폴더에 쓸 수 없으므로, 배경 이미지(`back1.png`~`back3.png`)와 오버레이 스크립트(`overlay1.txt`~`overlay4.txt`)는 `문서\ThePen` 폴더에서 읽습니다. zip 배포본은 기존처럼 실행 파일 옆에 둔 파일도 읽습니다.
