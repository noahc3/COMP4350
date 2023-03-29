import { observer } from "mobx-react-lite";
import { Box } from "@chakra-ui/react";
import ReactMarkdown from "react-markdown";
import ChakraUIRenderer from "chakra-ui-markdown-renderer";
import "./ThreaditMarkdown.css";

export const ThreaditMarkdown = observer(
  ({ text, className }: { text: string; className?: string }) => {
    return (
      <Box className={[className, "markdown-overrides"].join(" ")}>
        <ReactMarkdown
          components={ChakraUIRenderer()}
          disallowedElements={["img"]}
          children={text}
          skipHtml
        />
      </Box>
    );
  }
);
