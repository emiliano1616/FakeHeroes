using Assets.Scripts.Algorithms;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public List<UnitController> Heroes = new List<UnitController>();
        private UnitController SelectedHero;
        public UnitController BaseHero;
        private int HeroIndex = 0;

        private bool MustRedraw { get; set; }

        private Node PreviusMouseNode;
        private PathManager PathData;
        private GridBase Grid;


        public void Init(List<SaveableHero> savedHeros)
        {
            Heroes.RemoveAll(t => t == null);
            LoadHeros(savedHeros);

            Heroes.ForEach(t => t.Init());

            SelectedHero = Heroes.FirstOrDefault();
        }

        private void LoadHeros(List<SaveableHero> savedHeros)
        {
            if (savedHeros != null && savedHeros.Any())
            {
                Heroes.ForEach(t => GameObject.Destroy(t.gameObject));
                Heroes.Clear();
                savedHeros.ForEach(t =>
                {
                    var hero = Instantiate(BaseHero, GridBase.GetInstance.GetWorldCoordinatesFromNode(t.X, t.Y), Quaternion.identity);
                    hero.ActionPoints = t.ActionPoints;
                    hero.y = t.Y;
                    hero.x = t.X;
                    hero.name = "Hero i ";
                    Heroes.Add(hero);
                });
            }
        }

        private void Start()
        {
            Grid = GridBase.GetInstance;
            PathData = PathManager.GetInstance;
        }

        private void Update()
        {
            if (!Grid.IsInitialized) return;
            if (Heroes == null) return;
            if (this.SelectedHero == null) return;

            if (!SelectedHero.IsMoving && Input.GetKeyDown("h"))
            {
                SelectNextPlayer();
            }

            var path = PathData.GetAvailablePath();
            if (path != null && path.Count > 0
                && Input.GetMouseButtonUp(0))
            {
                SelectedHero.Move(path);
            }

            if (!SelectedHero.IsMoving)
                DrawPath();
        }

        private void SelectNextPlayer()
        {
            HeroIndex = HeroIndex == Heroes.Count - 1 ? 0 : HeroIndex + 1;
            SelectedHero = Heroes[HeroIndex];
            MustRedraw = true;
        }

        private void DrawPath()
        {
            var CurrentMouseNode = FindNodeUnderMouse();
            if (MustRedraw || (CurrentMouseNode != null && PreviusMouseNode != CurrentMouseNode))
            {
                MustRedraw = false;
                PreviusMouseNode = CurrentMouseNode;
                var playerPositionNode = Grid.GetNodeFromWorldPosition(SelectedHero.transform.position);

                var pf = new PathFinding(Grid.Grid, playerPositionNode, CurrentMouseNode);
                PathData.SetNewPath(pf.GetPath(), this.SelectedHero.ActionPoints);
            }
        }

        private Node FindNodeUnderMouse()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                return Grid.GetNodeFromWorldPosition(hit.point);

            return null;
        }


        public static GameManager GetInstance;
        private void Awake()
        {

            GetInstance = this;
        }
    }
}
