# Git 빠른 참조 카드 (Quick Reference)

## 🔍 상태 확인

```bash
# 현재 브랜치와 상태 확인
git status

# 현재 브랜치 이름만 확인
git rev-parse --abbrev-ref HEAD

# 모든 브랜치 및 추적 정보
git branch -vv

# 원격 저장소 확인
git remote -v

# 최근 커밋 히스토리 (그래프)
git log --oneline --graph --decorate -n 10
```

## 🌿 브랜치 작업

```bash
# 새 브랜치 생성 및 이동
git checkout -b feature/새기능

# origin/main 기준으로 새 브랜치 생성
git checkout -b feature/새기능 origin/main

# 브랜치 이동
git checkout 브랜치명

# 브랜치 삭제
git branch -d 브랜치명    # 병합된 브랜치만
git branch -D 브랜치명    # 강제 삭제

# 모든 브랜치 목록 (원격 포함)
git branch -a
```

## 💾 변경사항 관리

```bash
# 모든 변경사항 스테이징
git add .

# 특정 파일만 스테이징
git add 파일명

# 커밋
git commit -m "커밋 메시지"

# 마지막 커밋 수정
git commit --amend

# 마지막 커밋 취소 (변경사항 유지)
git reset --soft HEAD~1

# 마지막 커밋 취소 (변경사항 삭제)
git reset --hard HEAD~1
```

## 🔄 원격 동기화

```bash
# 최신 원격 정보 가져오기 (병합 안 함)
git fetch origin
git fetch --all --prune    # 모든 원격, 삭제된 브랜치 정리

# 원격 main 가져와서 병합
git pull origin main

# 원격에 푸시
git push origin 브랜치명

# 새 브랜치를 원격에 푸시 및 추적 설정
git push -u origin 브랜치명

# 안전한 강제 푸시 (rebase 후)
git push --force-with-lease origin 브랜치명
```

## 📦 Stash (임시 저장)

```bash
# 현재 변경사항 임시 저장
git stash push -m "작업 설명"

# Stash 목록 보기
git stash list

# 최근 stash 적용 및 삭제
git stash pop

# 특정 stash 적용 (삭제 안 함)
git stash apply stash@{0}

# Stash 내용 확인
git stash show -p stash@{0}

# Stash 삭제
git stash drop stash@{0}
```

## 🔀 Rebase

```bash
# main 최신 위로 현재 브랜치 재배치
git fetch origin
git rebase origin/main

# Rebase 중 충돌 해결 후 계속
git add 수정된파일
git rebase --continue

# Rebase 중단
git rebase --abort
```

## 🛡️ 백업 및 복구

```bash
# 백업 브랜치 생성
git branch backup/작업명-before-operation

# Reflog 확인 (모든 HEAD 변경 이력)
git reflog

# 특정 시점으로 복구
git reset --hard HEAD@{n}
git reset --hard 커밋해시
```

## 🔧 문제 해결

```bash
# 잘못된 파일 커밋 → 마지막 커밋 취소
git reset --soft HEAD~1

# 잘못된 브랜치에 커밋 → cherry-pick으로 이동
git checkout 올바른브랜치
git cherry-pick 커밋해시

# 충돌 발생 시 상태 확인
git status

# Merge/Rebase 중단
git merge --abort
git rebase --abort
```

## 📋 Main 브랜치 기준 작업 시나리오

### 시나리오 1: 새 기능 개발 시작
```bash
git fetch origin
git checkout -b feature/새기능 origin/main
# 작업 수행...
git add .
git commit -m "feat: 새 기능 추가"
git push -u origin feature/새기능
```

### 시나리오 2: 기존 브랜치를 main 최신으로 업데이트
```bash
git checkout 내브랜치
git branch backup/내브랜치-before-rebase  # 백업!
git fetch origin
git rebase origin/main
git push --force-with-lease origin 내브랜치
```

### 시나리오 3: 작업 중 급한 수정 필요
```bash
git stash push -m "현재 작업"
git checkout -b hotfix/긴급수정 origin/main
# 수정 작업...
git add .
git commit -m "fix: 긴급 수정"
git push -u origin hotfix/긴급수정
# 원래 작업으로 복귀
git checkout 원래브랜치
git stash pop
```

## ⚠️ 주의사항

### ❌ 절대 사용하지 말 것
```bash
git push --force origin main    # 위험! 팀원 작업 손실 가능
git reset --hard                # 백업 없이 사용 금지
```

### ✅ 대신 이것을 사용
```bash
git push --force-with-lease origin 브랜치명
git branch backup/브랜치명-before-operation  # 항상 백업 먼저!
```

## 📚 더 자세한 내용

전체 가이드는 [Git 워크플로우 가이드](git-workflow-guide.md)를 참조하세요.

---

**💡 팁**: 이 명령어들을 터미널에 직접 복사해서 사용할 수 있습니다!
