import { observer } from "mobx-react";
import { navStore } from "../../stores/NavStore";

interface INavLink {
  to: string;
  children?: React.ReactNode;
}

export const NavLink = observer(({ to, children }: INavLink) => {
  const selectedPath = navStore.currentPath;
  const onClick = (e: React.MouseEvent) => {
    e.preventDefault();
    navStore.navigateTo(to);
  };

  return (
    <div className={selectedPath === to ? "selected" : ""} onClick={onClick}>
      {children}
    </div>
  );
});
