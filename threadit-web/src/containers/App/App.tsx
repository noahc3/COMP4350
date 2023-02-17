import { Box, Flex } from '@chakra-ui/react';
import { observer } from 'mobx-react';
import { AuthGate } from '../AuthGate/AuthGate';
import { Router } from '../Router/Router';
import { Sidebar } from '../Sidebar/Sidebar';
import './App.css';

const App = observer(() => {
    return (
        <div className="App">
            <AuthGate>
                <Flex>
                    <Box flexBasis={"content"}>
                        <Sidebar />
                    </Box>
                    <Box flexGrow={1} maxH="100vh" scrollBehavior={"smooth"}>
                        <Router />
                    </Box>
                </Flex>
            </AuthGate>
        </div>
    );
});

export default App;
