VAR playerSocialReputation = -52

자네 외래 생활은 어떤가?
    *[솔직히 제 적성이 아닌 것 같습니다.]
    -> courage
    *[할만합니다.]
    -> courage
    *[아직 잘 모르겠습니다.]
    -> courage

=== courage ===
{playerSocialReputation < -50 :
    아, 그런가. 그나저나 자네 외래 진료 환자가 남긴 만족도가 왜 이렇게 낮나? 조금 더 신경써서 진료하게.
    자네에 대한 안 좋은 말이 많아. 환자들 입소문이 빨라서 평판도 신경 써야할 걸세
}
{playerSocialReputation > -51 && playerSocialReputation < 50 :
    아, 그런가. 그나저나 자네 외래 진료에 나름 잘 적응해 나가고 있더군. 어떻게 아냐고? 하하, 나는 다 통로가 있네. 이대로면 금방 우리 병원 최고의 의사가 되겠어.
}
{playerSocialReputation > 49 :
    아, 그런가. 그나저나 자네 아주 진료를 잘 본다고 동네에 소문이 자자하더군. 잘하고 있네, 아주 훌륭해.
}
->END